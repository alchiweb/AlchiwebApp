using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AlchiwebApp.Cli.Core.Services;

namespace AlchiwebApp.Cli.Core;

public abstract partial class BitPlatformApp
{
    static readonly string TEMPLATE_PROJECT_NAME = "Alchiweb-App1";
    protected string BitPlatformProjectFolder { get; } = "";
    protected string SourceProjectFolder { get; } = "";
    protected string ProjectName { get; } = "";
    protected List<string> Errors { get; } = [];
    protected bool UseExpectedReplacements { get; }

    private readonly FileSearchService _searchService;
    public BitPlatformApp(string bitPlatformProjectFolderPath, string sourceProjectFolder, bool useExpectedReplacements, FileSearchService searchService)
    {
        BitPlatformProjectFolder = Path.TrimEndingDirectorySeparator(bitPlatformProjectFolderPath);
        ProjectName = Path.GetFileName(BitPlatformProjectFolder);
        SourceProjectFolder = Path.TrimEndingDirectorySeparator(sourceProjectFolder);
        UseExpectedReplacements = useExpectedReplacements;
        _searchService = searchService;
    }

    protected XElement? AddItemGroup(XDocument sourceXDoc)
    {
        var projectElement = sourceXDoc?.Element("Project");
        if (projectElement == null)
        {
            Errors.Add("ItemGroup cannot be created.");
            return null;
        }
        XElement itemGroupToAdd = new("ItemGroup");
        projectElement.Add(itemGroupToAdd);
        return itemGroupToAdd;
    }

    protected List<XElement> GetRessourcesItemsFromCsproj(XDocument sourceXDoc, string updateValue)
    {
        //var items = sourceXDoc.Document?.Element("Project")?.Elements("ItemGroup")?.SelectMany(
        //    el => el?.Elements("EmbeddedResource") ?? new List<XElement>(), (el, c) => c)
        //    //.Where(e =>
        //    //    {
        //    //        var attributeValue = e?.Attribute("Update")?.Value;
        //    //        return attributeValue != null;// && attributeValue.StartsWith(updateValue);
        //    //    }
        //    //)
        //    .ToList();

        var items = sourceXDoc.Descendants("EmbeddedResource")
            .Where(e =>
            {
                var attributeValue = e?.Attribute("Update")?.Value;
                return attributeValue != null && attributeValue.StartsWith(updateValue);
            }
            )
            .ToList();
        return items;
    }

    protected async Task<List<string>> CopyFilesRecursivelyAsync(string sourcePath, string targetPath, bool isTemplateDirectory, string? excludeFilesPattern = null, string? excludeDirectory = null)
    {
        var sourceDirectories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
            .Where(filename => excludeDirectory == null || !filename.Split([Path.DirectorySeparatorChar]).Contains(excludeDirectory))
            ;
        var sourceFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
            .Where(filename => (string.IsNullOrEmpty(excludeFilesPattern) || !filename.Contains(excludeFilesPattern)) && (excludeDirectory == null || !filename.Split([Path.DirectorySeparatorChar]).Contains(excludeDirectory)))
            ;
        // Create all of the directories
        foreach (string dirPath in sourceDirectories)
        {
            var destPath = dirPath.Replace(sourcePath, targetPath);
            if (isTemplateDirectory)
                destPath = destPath.Replace(TEMPLATE_PROJECT_NAME, ProjectName);
            Directory.CreateDirectory(destPath);
        }

        List<string> copiedFiles = new();
        // Copy all the files & Replaces any files with the same name
        foreach (string newPath in sourceFiles)
        {
            var destPath = newPath.Replace(sourcePath, targetPath);
            if (isTemplateDirectory)
                destPath = destPath.Replace(TEMPLATE_PROJECT_NAME, ProjectName);
            File.Copy(newPath, destPath, false);
            copiedFiles.Add(destPath);
        }

        if (isTemplateDirectory)
        {
            await ReplaceTextAsync(TEMPLATE_PROJECT_NAME, ProjectName, directories: [targetPath]);
        }

        return copiedFiles;
    }

    protected List<string> MoveFilesRecursively(string sourcePath, string targetPath, bool deleteEmptyDirectory, string? excludeFilesPattern = null, string? excludeDirectory = null)
    {
        var sourceDirectories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).Where(filename => excludeDirectory == null || !filename.Split([Path.DirectorySeparatorChar]).Contains(excludeDirectory));
        var sourceFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories).Where(filename => (string.IsNullOrEmpty(excludeFilesPattern) || !filename.Contains(excludeFilesPattern)) && (excludeDirectory == null || !filename.Split([Path.DirectorySeparatorChar]).Contains(excludeDirectory)));
        // Create all of the directories
        foreach (string dirPath in sourceDirectories)
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        List<string> movedFiles = new();
        // Copy all the files & Replaces any files with the same name
        foreach (string newPath in sourceFiles)
        {
            var destPath = newPath.Replace(sourcePath, targetPath);
            File.Move(newPath, destPath, false);
            movedFiles.Add(destPath);
        }
        if (deleteEmptyDirectory)
        {
            foreach (var dir in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).Reverse())
            {
                try
                {
                    Directory.Delete(dir);
                }
                catch { }
            }
        }
        return movedFiles;
    }

    protected void MoveOrRenameFiles(
        string sourcePattern,
        string targetDirectoryPath
    )
    {
        string[] filenames = Directory.GetFiles(Path.GetDirectoryName(sourcePattern), Path.GetFileName(sourcePattern));
        foreach (string filename in filenames)
        {
            MoveOrRenameFile(filename, Path.Combine(targetDirectoryPath, Path.GetFileName(filename)));
        }
    }

    protected async Task<int> ReplaceTextAsync(string searchText, string replaceText, bool matchCase = false, bool matchWholeWord = false, string[]? directories = null, string[]? filters = null, int? expectedReplacements = null, string[]? excludeDirectories = null)
    {
        if (directories == null || directories.Length == 0)
        {
            directories = [BitPlatformProjectFolder];
        }
        if (excludeDirectories == null)
        {
            excludeDirectories = ["bin", "obj"];
        }
        if (filters == null || filters.Length == 0)
        {
            filters = ["*.*"];
        }
        var listFiles = await _searchService.SearchFilesAsync(directories, filters, true, searchText, matchCase, matchWholeWord, excludeDirectories: excludeDirectories);
        if (listFiles.Count != 0)
        {
            await _searchService.ReplaceInFilesAsync(searchText, replaceText, listFiles, matchCase);
        }
        if (UseExpectedReplacements && expectedReplacements != null && (expectedReplacements >= 0 ? listFiles.Count != expectedReplacements : listFiles.Count < -expectedReplacements))
        {
            Errors.Add($@"Replacements for ""{searchText}"" ({string.Join('|', filters)}): {listFiles.Count} / Expected: {(expectedReplacements >= 0 ? expectedReplacements : $">= {-expectedReplacements}")}");
        }
        return listFiles.Count;
    }
    protected bool MoveOrRenameDirectory(string oldRelativePath, string newRelativePath)
    {
        var oldFullPath = Path.Combine(BitPlatformProjectFolder, oldRelativePath);
        var newFullPath = Path.Combine(BitPlatformProjectFolder, newRelativePath);
        if (Directory.Exists(newFullPath))
        {
            Errors.Add($@"The directory ""{newRelativePath}"" already exists.");
            return true;
        }

        try
        {
            Directory.Move(oldFullPath, newFullPath);

        }
        catch (Exception)
        {
            Errors.Add($@"The directory ""{oldRelativePath}"" cannot be moved (or renamed).");
            return false;
        }
        return true;
    }
    protected bool MoveOrRenameFile(string oldRelativePath, string newRelativePath)
    {
        var oldFullPath = Path.Combine(BitPlatformProjectFolder, oldRelativePath);
        var newFullPath = Path.Combine(BitPlatformProjectFolder, newRelativePath);
        if (File.Exists(newFullPath))
        {
            Errors.Add($@"The file ""{newRelativePath}"" already exists.");
            return true;
        }
        try
        {
            File.Move(oldFullPath, newFullPath);
        }
        catch (Exception)
        {
            Errors.Add($@"The file ""{oldRelativePath}"" cannot be moved (or renamed).");
            return false;
        }
        return true;
    }
}
