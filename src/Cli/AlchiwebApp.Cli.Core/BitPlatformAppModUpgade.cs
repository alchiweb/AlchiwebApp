using System.Reflection;
using System.Windows.Input;
using System.Xml.Linq;
using AlchiwebApp.Cli.Core.Services;

namespace AlchiwebApp.Cli.Core;

public partial class BitPlatformAppModUpgrade : BitPlatformApp
{
    private readonly FileSearchService _searchService;

    public BitPlatformAppModUpgrade(string bitPlatformProjectFolderPath, bool useExpectedReplacements, FileSearchService searchService)
        : base(bitPlatformProjectFolderPath, bitPlatformProjectFolderPath, useExpectedReplacements, searchService)
    {
        _searchService = searchService;
    }

    public async Task AddAlchiwebApp()
    {
        Console.WriteLine($"Project name: {ProjectName} / Project directory: {BitPlatformProjectFolder}");

        try
        {
            ModifyCsProjFiles();
            ModifySolutionFile();
            await CopyAlchiwebAppFilesAsync();
            AddGit();
        }
        catch (Exception ex)
        {
            Errors.Add($@"Aborted: critical error ({ex.Message}).");
        }
        
        if (Errors.Count > 0)
        {
            Console.WriteLine($"Done with errors/warnings:");
            foreach (var error in Errors)
            {
                Console.WriteLine($" - Error: {error}");
            }
        }
        else
        {
            Console.WriteLine($"Done without error.");
        }

    }
    private void ModifyCsProjFiles()
    {
        string sourceProject = $"{ProjectName}.Core";
        string sourceResourcesProjectFile = Path.Combine(
            BitPlatformProjectFolder, "src", $"{sourceProject}", $"{sourceProject}.csproj"
            );
        var sourceXDoc = XDocument.Load(sourceResourcesProjectFile);

        var listSourceResources = CopyRessources("AppStrings",
            "I18n",
            sourceProject,
            sourceXDoc
            );

        var itemGroupToAdd = AddItemGroup(sourceXDoc);
        if (itemGroupToAdd != null)
        {
            var projectReferenceToAdd = new XElement("ProjectReference");
            projectReferenceToAdd.SetAttributeValue("Include", Path.Combine(
                "..", "..", "AlchiwebApp", "src", "AlchiwebApp.Core", "AlchiwebApp.Core.csproj"
                ));
            itemGroupToAdd.Add(projectReferenceToAdd);
        }
        
        sourceXDoc?.SaveXmlFile(sourceResourcesProjectFile);

        sourceProject = $"{ProjectName}.Client.Core";
        sourceResourcesProjectFile = Path.Combine(
            BitPlatformProjectFolder, "src", "Client", $"{sourceProject}", $"{sourceProject}.csproj"
            );
        sourceXDoc = XDocument.Load(sourceResourcesProjectFile);

        CopyRessources("AppStrings",
            "ClientI18n",
            sourceProject,
            sourceXDoc,
            sourceItems: listSourceResources
            );

        itemGroupToAdd = AddItemGroup(sourceXDoc);
        if (itemGroupToAdd != null)
        {
            var projectReferenceToAdd = new XElement("ProjectReference");
            projectReferenceToAdd.SetAttributeValue("Include", Path.Combine(
                "..", "..", "..", "AlchiwebApp", "src", "AlchiwebApp.Client.Core", "AlchiwebApp.Client.Core.csproj"
                ));
            itemGroupToAdd.Add(projectReferenceToAdd);
        }
        itemGroupToAdd = AddItemGroup(sourceXDoc);
        if (itemGroupToAdd != null)
        {
            var packageReferenceToAdd = new XElement("PackageReference");
            packageReferenceToAdd.SetAttributeValue("Include", "Blazored.LocalStorage");
            itemGroupToAdd.Add(packageReferenceToAdd);
        }
        itemGroupToAdd = AddItemGroup(sourceXDoc);
        if (itemGroupToAdd != null)
        {
            var usingToAdd = new XElement("Using");
            usingToAdd.SetAttributeValue("Include", "AlchiwebApp.Core");
            itemGroupToAdd.Add(usingToAdd);
            usingToAdd = new XElement("Using");
            usingToAdd.SetAttributeValue("Include", "AlchiwebApp.Core.Interfaces");
            itemGroupToAdd.Add(usingToAdd);
            usingToAdd = new XElement("Using");
            usingToAdd.SetAttributeValue("Include", "AlchiwebApp.PagingFiltering.Filtering");
            itemGroupToAdd.Add(usingToAdd);
            usingToAdd = new XElement("Using");
            usingToAdd.SetAttributeValue("Include", "AlchiwebApp.PagingFiltering.Filtering.Enums");
            itemGroupToAdd.Add(usingToAdd);
            usingToAdd = new XElement("Using");
            usingToAdd.SetAttributeValue("Include", "AlchiwebApp.PagingFiltering.Paging");
            itemGroupToAdd.Add(usingToAdd);
        }
        sourceXDoc?.SaveXmlFile(sourceResourcesProjectFile);

        sourceResourcesProjectFile = Path.Combine(
            BitPlatformProjectFolder, "src", "Directory.Packages.props"
            );
        sourceXDoc = XDocument.Load(sourceResourcesProjectFile);

        itemGroupToAdd = AddItemGroup(sourceXDoc);
        if (itemGroupToAdd != null)
        {
            var packageVersion = new XElement("PackageVersion");
            packageVersion.SetAttributeValue("Include", "Blazored.LocalStorage");
            packageVersion.SetAttributeValue("Version", "4.5.0");
            itemGroupToAdd.Add(packageVersion);
        }
        sourceXDoc?.SaveXmlFile(sourceResourcesProjectFile);

    }

    private void ModifySolutionFile()
    {
        string solutionFile = Path.Combine(BitPlatformProjectFolder, $"{ProjectName}.slnx");
        var sourceXDoc = XDocument.Load(solutionFile);

        var solutionElement = sourceXDoc?.Element("Solution");
        if (solutionElement == null)
        {
            Errors.Add("Solution file cannot be read.");
            return;
        }
        XElement folderToAdd = new("Folder");
        folderToAdd.SetAttributeValue("Name", "/AlchiwebApp/");
        solutionElement.AddFirst(folderToAdd);

        XElement projectToAdd = new("Project");
        projectToAdd.SetAttributeValue("Path", "AlchiwebApp/src/AlchiwebApp.Core/AlchiwebApp.Core.csproj");
        folderToAdd.AddFirst(projectToAdd);
        projectToAdd = new("Project");
        projectToAdd.SetAttributeValue("Path", "AlchiwebApp/src/AlchiwebApp.PagingFiltering/AlchiwebApp.PagingFiltering.csproj");
        folderToAdd.AddFirst(projectToAdd);
        projectToAdd = new("Project");
        projectToAdd.SetAttributeValue("Path", "AlchiwebApp/src/Ardalis.Specification.EntityFrameworkCore/Ardalis.Specification.EntityFrameworkCore.csproj");
        folderToAdd.AddFirst(projectToAdd);
        projectToAdd = new("Project");
        projectToAdd.SetAttributeValue("Path", "AlchiwebApp/src/AlchiwebApp.Client.Core/AlchiwebApp.Client.Core.csproj");
        folderToAdd.AddFirst(projectToAdd);
        projectToAdd = new("Project");
        projectToAdd.SetAttributeValue("Path", "AlchiwebApp/src/AlchiwebApp.Server.Core/AlchiwebApp.Server.Core.csproj");
        folderToAdd.AddFirst(projectToAdd);

        sourceXDoc?.SaveXmlFile(solutionFile);
    }


    private async Task CopyAlchiwebAppFilesAsync()
    {
        var sourceFolder = Assembly.GetEntryAssembly()?.Location;
        if (string.IsNullOrEmpty(sourceFolder))
            return;
        sourceFolder = Path.GetDirectoryName(sourceFolder);
        if (string.IsNullOrEmpty(sourceFolder))
            return;
        sourceFolder = Path.Combine(sourceFolder, "Content");
        if (string.IsNullOrEmpty(sourceFolder))
            return;
        await CopyFilesRecursivelyAsync(sourceFolder, BitPlatformProjectFolder, true);
    }

    private void AddGit()
    {
        var gitFolderPath = Path.Combine(BitPlatformProjectFolder, ".git");
        if (Directory.Exists(gitFolderPath))
        {
            Errors.Add("The target folder already contains a .git folder.");
            return;
        }
        ProcessStartInfo gitCommand = new("git")
        {
            WorkingDirectory = BitPlatformProjectFolder
        };

        //gitCommand.Arguments = "clone --recurse-submodules https://github.com/alchiweb/AlchiwebApp.git AlchiwebApp";
        //Process.Start(gitCommand)?.WaitForExit();
        gitCommand.Arguments = "init";
        Process.Start(gitCommand)?.WaitForExit();

        gitCommand.Arguments = "submodule add https://github.com/alchiweb/AlchiwebApp.git AlchiwebApp";
        Process.Start(gitCommand)?.WaitForExit();

        gitCommand.Arguments = "submodule update --init --recursive";
        Process.Start(gitCommand)?.WaitForExit();

        gitCommand.Arguments = "add .";
        Process.Start(gitCommand)?.WaitForExit();
        gitCommand.Arguments = @"commit -m ""AlchiwebApp generated version""";
        Process.Start(gitCommand)?.WaitForExit();
    }
    private List<XElement>? CopyRessources(string resourcesToGet,
        string resourcesToAdd,
        string sourceProject,
        XDocument sourceXDoc,
        List<XElement>? sourceItems = null)
    {
        bool hasListItems = sourceItems != null;
        if (!hasListItems && string.IsNullOrEmpty(resourcesToGet))
        {
            Errors.Add("Resource source must be provided.");
            return null;
        }
        string resourcesToGetWithDot = $"{resourcesToGet}.";
        string resourcesToAddWithDot = $"{resourcesToAdd}.";
        string sourceResourcesDirectory = "Resources";

        if (sourceItems == null)
        {
            sourceItems = GetRessourcesItemsFromCsproj(sourceXDoc, Path.Combine(sourceResourcesDirectory, resourcesToGetWithDot));
        }
        if (sourceItems == null || sourceItems.Count == 0)
        {
            Errors.Add("ItemGroup section not found in source csproj file (for moving resources files).");
            return null;
        }
        List<XElement> itemsToAdd = new();
        foreach (var item in sourceItems)
        {
            var newItem = new XElement(item);
            var attributeWithResourcesPath = newItem.Attribute("Update");
            if (attributeWithResourcesPath != null)
            {
                attributeWithResourcesPath.Value = attributeWithResourcesPath.Value.Replace(resourcesToGetWithDot, resourcesToAddWithDot);
            }
            var elementWithResourcesPath = newItem.Element("LastGenOutput");
            if (elementWithResourcesPath != null)
            {
                elementWithResourcesPath.Value = elementWithResourcesPath.Value.Replace(resourcesToGetWithDot, resourcesToAddWithDot);
            }
            elementWithResourcesPath = newItem.Element("DependentUpon");
            if (elementWithResourcesPath != null)
            {
                elementWithResourcesPath.Value = elementWithResourcesPath.Value.Replace(resourcesToGetWithDot, resourcesToAddWithDot);
            }
            elementWithResourcesPath = newItem.Element("StronglyTypedFileName");
            if (elementWithResourcesPath != null)
            {
                elementWithResourcesPath.Value = elementWithResourcesPath.Value.Replace(resourcesToGetWithDot, resourcesToAddWithDot);
            }
            elementWithResourcesPath = newItem.Element("StronglyTypedClassName");
            if (elementWithResourcesPath != null)
            {
                elementWithResourcesPath.Value = elementWithResourcesPath.Value.Replace(resourcesToGet, resourcesToAdd);
            }
            itemsToAdd.Add(newItem);
        }
        if (hasListItems)
        {
            var  itemGroupToAdd = AddItemGroup(sourceXDoc);
            if (itemGroupToAdd == null)
            {
                return sourceItems;
            }
            itemGroupToAdd.Add(itemsToAdd);
        }
        else
        {
            sourceItems.First().AddBeforeSelf(itemsToAdd);
        }

        return sourceItems;
    }
}
