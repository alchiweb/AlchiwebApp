using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Reflection;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using AlchiwebApp.Console.AddToBitPlatform;
using AlchiwebApp.Console.AddToBitPlatform.Extensions;
using AlchiwebApp.Console.Core.Services;
using Rejigs;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    static string _bitPlatformProjectFolder = "";
    static string _sourceProjectFolder = "";
    static string _projectName = "";
    static readonly FileSearchService _searchService = new();
    static List<string> _errors = [];
    static bool _useExpectedReplacements = false;
    private static async Task<int> Main(string[] args)
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        Option<DirectoryInfo> targetDirectoryOption = new("--target", "-t")
        {
            Description = "The folder of the newly created BitPlatform application (to modify)",
            Required = true,
            //DefaultValueFactory = parseResult => new DirectoryInfo("."),
            HelpName = "BitPlatform folder",
            Arity = ArgumentArity.ExactlyOne,
            Validators =
            {
                optionResult => DirectoryValidation(optionResult)
            }
        };
        Option<DirectoryInfo> sourceDirectoryOption = new("--source", "-s")
        {
            Description = "The folder of the source BitPlatform application (to read parameters)",
            Required = true,
            //DefaultValueFactory = parseResult => new DirectoryInfo(Path.Combine("..", "..")),
            HelpName = "BitPlatform folder",
            Arity = ArgumentArity.ExactlyOne,
            Validators =
            {
                optionResult => DirectoryValidation(optionResult)
            }
        };
        Console.WriteLine($"AddToBitPlatform v{assemblyVersion}");
        RootCommand rootCommand = new($"AlchiwebApp / Add To BitPlatform v{assemblyVersion} (tested with BitPlatform v10.4.4 / v14.4.5)");
        rootCommand.Options.Add(targetDirectoryOption);
        rootCommand.Options.Add(sourceDirectoryOption);
        rootCommand.SetAction(async (parseResult) =>
        {
            if (parseResult.GetRequiredValue(targetDirectoryOption) is DirectoryInfo targetParsedDirectory
                && parseResult.GetRequiredValue(sourceDirectoryOption) is DirectoryInfo sourceParsedDirectory
            )
            {
                await ModifyBitPlatformProject(targetParsedDirectory.FullName, sourceParsedDirectory.FullName);
            }
        });

        for (int i = 0; i < rootCommand.Options.Count; i++)
        {
            // RootCommand has a default VersionOption; update its Action.
            if (rootCommand.Options[i] is VersionOption defaultVersionOption)
            {
                defaultVersionOption.Action = new CustomVersionAction();
                break;
            }
        }

        ParseResult parseResult = rootCommand.Parse(args);
        parseResult.Invoke();

        return parseResult.Errors.Count == 0 ? 0 : 1;
    }

    private static bool DirectoryValidation(OptionResult optionResult)
    {
        DirectoryInfo? optionValue = null;
        try
        {
            optionValue = optionResult.GetValueOrDefault<DirectoryInfo>();
            if (Directory.Exists(optionValue.FullName) == false)
            {
                optionResult.AddError("Folder does not exist.");
                return false;
            }
        }
        catch (Exception)
        {
            optionResult.AddError("Invalid folder path.");
            return false;
        }

        return true;
    }

    private static async Task ModifyBitPlatformProject(string bitPlatformProjectFolderPath, string sourceProjectFolder)
    {
        _bitPlatformProjectFolder = Path.TrimEndingDirectorySeparator(bitPlatformProjectFolderPath);
        _projectName = Path.GetFileName(_bitPlatformProjectFolder);
        _sourceProjectFolder = Path.TrimEndingDirectorySeparator(sourceProjectFolder);
        Console.WriteLine($"Project name: {_projectName} / Project directory: {_bitPlatformProjectFolder}");

        int[]? bitPlatformForwardPorts = null;
        int[]? sourceForwardPorts = null;
        string bitPlatformUserSecrets = "";
        string sourceUserSecrets = "";

        bool hasSourceBitPlatformProject = !string.Equals(_bitPlatformProjectFolder, _sourceProjectFolder);

        if (hasSourceBitPlatformProject)
        {
            sourceForwardPorts = ReadForwardPorts(_sourceProjectFolder);
            bitPlatformForwardPorts = ReadForwardPorts(_bitPlatformProjectFolder);
            sourceUserSecrets = ReadUserSecrets(_sourceProjectFolder);
            bitPlatformUserSecrets = ReadUserSecrets(_bitPlatformProjectFolder);
        }
        if (_errors.Count > 0)
        {
            _errors.Add($@"Aborted: critical error.");
        }
        else
        {
            try
            {
                await RenameServerSharedProjectAsync(true);
                await RenameSharedProjectAsync(true);

                if (hasSourceBitPlatformProject)
                {
                    await ReplaceForwardedPortsAndUserSecretsAsync(bitPlatformForwardPorts, bitPlatformUserSecrets, sourceForwardPorts, sourceUserSecrets);
                }

                await MoveFilesToServerCoreProjectAsync();
            }
            catch (Exception ex)
            {
                _errors.Add($@"Aborted: critical error ({ex.Message}).");
            }
        }
        if (_errors.Count > 0)
        {
            Console.WriteLine($"Done with errors/warnings:");
            foreach (var error in _errors)
            {
                Console.WriteLine($" - Error: {error}");
            }
        }
        else
        {
            Console.WriteLine($"Done without error.");
        }

    }
    private static async Task MoveFilesToServerCoreProjectAsync()
    {
        string sourceCorePath = Path.Combine(_bitPlatformProjectFolder, "src", $"{_projectName}.Core");
        string sourceServerCorePath = Path.Combine(_bitPlatformProjectFolder, "src", "Server", $"{_projectName}.Server.Core");
        string sourceServerApiPath = Path.Combine(_bitPlatformProjectFolder, "src", "Server", $"{_projectName}.Server.Api");
        string sourceServerWebPath = Path.Combine(_bitPlatformProjectFolder, "src", "Server", $"{_projectName}.Server.Web");
        string sourceTestsPath = Path.Combine(_bitPlatformProjectFolder, "src", "Tests");

        MoveOrRenameFile(
            Path.Combine(sourceCorePath, "Infrastructure", "Extensions", "ActivitySourceExtensions.cs"),
            Path.Combine(sourceServerCorePath, "Infrastructure", "Extensions", "ActivitySourceExtensions.cs")
            );
        MoveOrRenameFile(
            Path.Combine(sourceCorePath, "Infrastructure", "Extensions", "MeterExtensions.cs"),
            Path.Combine(sourceServerCorePath, "Infrastructure", "Extensions", "MeterExtensions.cs")
        );

        MoveResources();

        string sourcePath = Path.Combine(sourceServerApiPath, "Features");

        MoveOrRenameFile(
            Path.Combine(sourceServerApiPath, "ServerApiSettings.cs"),
            Path.Combine(sourceServerCorePath, "ServerCoreSettings.cs")
            );
        await ReplaceTextAsync("ServerApiSettings", "ServerCoreSettings", true, true, [
            sourceServerApiPath,
            sourceServerCorePath
            ]);

        MoveFilesRecursively(
            sourcePath,
            Path.Combine(sourceServerCorePath, "Features"),
            true,
            "Controller."
            ).Select(filename => new SearchResult() { FilePath = filename }).ToList();
        sourcePath = Path.Combine(sourceServerApiPath, "Infrastructure");

        // TODO: merge to two files (with regex)
        var extensionsCsFile1 = Path.Combine("Infrastructure", "Extensions", "HttpContextExtensions");
        var extensionsCsFile2 = Path.Combine(sourceServerApiPath, "Infrastructure", "Extensions", "HttpRequestExtensions");
        MoveOrRenameFile(
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile1}.cs"),
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile1}.FromApi.cs")
            );
        // TODO: merge to two files (with regex)
        MoveOrRenameFile(
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile2}.cs"),
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile2}.FromApi.cs")
            );
        string[] arrayExtensionsFiles = [
            Path.Combine(sourceServerCorePath, $"{extensionsCsFile1}.cs"),
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile1}.FromApi.cs"),
            Path.Combine(sourceServerCorePath, $"{extensionsCsFile2}.cs"),
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile2}.FromApi.cs")
            ];
        var listExtensionsFiles = arrayExtensionsFiles.Select(text => new SearchResult() { FilePath = text }).ToList();
        await _searchService.ReplaceInFilesAsync("public static class", "public static partial class", listExtensionsFiles, true, true);
        await _searchService.ReplaceInFilesAsync("internal static class", "public static partial class", listExtensionsFiles, true, true);

        MoveFilesRecursively(
            sourcePath,
            Path.Combine(sourceServerCorePath, "Infrastructure"),
            true,
            excludeDirectory: "Controllers"
            ).Select(filename => new SearchResult() { FilePath = filename }).ToList();

        ChangeCsprojFiles(sourceServerCorePath, sourceServerApiPath);

        await ReplaceTextAsync($@"{_projectName}.Server.Api", $@"{_projectName}.Server.Core", true, true,
            [ sourceServerCorePath, sourceTestsPath ]);

        await ReplaceTextAsync($@"using {_projectName}.Server.Api.Infrastructure", $@"using {_projectName}.Server.Core.Infrastructure", true, true,
            [sourceServerApiPath, sourceServerWebPath]);
        await ReplaceTextAsync($@"using {_projectName}.Server.Api.Features", $@"using {_projectName}.Server.Core.Features", true, true,
            [sourceServerApiPath, sourceServerWebPath]);
        //await _searchService.ReplaceInFilesAsync($"(\\n\\n^namespace ({_projectName}\\.Server\\.)Api(\\.Features\\..*);$)", "\\nusing $2Core$3;$1",
        //    [new SearchResult() { FilePath = Path.Combine(sourceServerApiPath, "Features", "*.*")}],
        //    useRegex: true);
        //await _searchService.ReplaceInFilesAsync($"(\\n\\n^namespace ({_projectName}\\.Server\\.)Api(\\.Infrastruture\\..*);$)", "\\nusing $2Core$3;$1",
        //    [new SearchResult() { FilePath = Path.Combine(sourceServerApiPath, "Infrastructure", "*.*") }],
        //    useRegex: true);
        //await _searchService.ReplaceInFilesAsync($"(\\n\\n^namespace ({_projectName}\\.Server\\.)Api;$)", "\\nusing $2Core;$1",
        //    [new SearchResult() { FilePath = Path.Combine(sourceServerApiPath, "Program.*") }],
        //    useRegex:true);
        await ReplaceTextAsync("<Infrastructure.SignalR.", "<Core.Infrastructure.SignalR.", true, false,
            [sourceServerApiPath, sourceServerWebPath],
            ["Program*.*"]);
        await ReplaceTextAsync("<Api.Infrastructure.SignalR.", "<Core.Infrastructure.SignalR.", true, false,
            [sourceServerApiPath, sourceServerWebPath],
            ["Program*.*"]);
        await ReplaceTextAsync(" Features.Identity.Models.", " Core.Features.Identity.Models.", true, false,
            [sourceServerApiPath, sourceServerWebPath],
            ["Program*.*"]);
        await ReplaceTextAsync(" Api.Features.Identity.Models.", " Core.Features.Identity.Models.", true, false,
            [sourceServerApiPath, sourceServerWebPath],
            ["Program*.*"]);
        try
        {
            Directory.Delete(Path.Combine(sourceTestsPath, "bin"), true);
            Directory.Delete(Path.Combine(sourceTestsPath, "obj"), true);
        }
        catch (Exception) { }
    }

    private static void ChangeCsprojFiles(string sourceServerCorePath, string sourceServerApiPath)
    {
        var serverApiCsprojPath = Path.Combine(sourceServerApiPath, $"{_projectName}.Server.Api.csproj");
        var serverCoreCsprojPath = Path.Combine(sourceServerCorePath, $"{_projectName}.Server.Core.csproj");
        var serverApiXDoc = XDocument.Load(serverApiCsprojPath);
        var serverCoreXDoc = XDocument.Load(serverCoreCsprojPath);
        List<XElement> usingsListForApi = serverApiXDoc.Descendants("Using").ToList();
        //List<XElement> newUsingsListForCore = serverCoreXDoc.Descendants("Using").ToList();
        List<string> usingsTextListForCore = serverCoreXDoc.Descendants("Using")
            .Select(elt => elt?.Attribute("Include")?.Value)
            .Where(val => !string.IsNullOrEmpty(val)).ToList();
        string controllersUsing = $"{_projectName}.Server.Api.Infrastructure.Controllers";
        List<XElement> controllersUsings = usingsListForApi.Where(u => u.Attribute("Include")?.Value.StartsWith(controllersUsing) == true).ToList();

        foreach (var us in usingsListForApi.Except(controllersUsings))
        {
            var includeAttribute = us.Attribute("Include");
            if (includeAttribute?.Value != null &&
                    (
                        includeAttribute.Value.StartsWith($"{_projectName}.Server.Api.Features") ||
                        includeAttribute.Value.StartsWith($"{_projectName}.Server.Api.Infrastructure")
                    )
                )
            {
                includeAttribute.Value = includeAttribute.Value.Replace($"{_projectName}.Server.Api", $"{_projectName}.Server.Core");
            }
        }

        var newUsingsListForCore = new List<XElement>();
        newUsingsListForCore = usingsListForApi.Distinct().Except(controllersUsings).Where(us =>
        {
            var includeValueAttribute = us.Attribute("Include")?.Value;
            if (includeValueAttribute == null || usingsTextListForCore.Contains(includeValueAttribute))
                return false;
            var moveToCore = MustMoveToServerCore(includeValueAttribute);
            
            if (moveToCore && !includeValueAttribute.StartsWith($"{_projectName}.Server.Core"))
            {
                us.Remove();
            }
            return moveToCore;
        }
            ).ToList();

        List<XElement> packageReferencesListForApi = serverApiXDoc.Descendants("PackageReference").ToList();
        //List<XElement> packageReferencesListForCore = serverCoreXDoc.Descendants("PackageReference").ToList();
        List<string> packageReferencesTextListForCore = serverCoreXDoc.Descendants("PackageReference")
            .Select(elt => elt?.Attribute("Include")?.Value)
            .Where(val => !string.IsNullOrEmpty(val)).ToList();
        var newPackageReferencesCore = packageReferencesListForApi.Distinct().Where(pr =>
        {
            var includeValueAttribute = pr.Attribute("Include")?.Value;
            if (includeValueAttribute == null || packageReferencesTextListForCore.Contains(includeValueAttribute))
                return false;
            var moveToCore = MustMoveToServerCore(includeValueAttribute);
            if (moveToCore)
            {
                pr.Remove();
            }
            return moveToCore;
        }
            ).ToList();

        var serverCoreProject = serverCoreXDoc.Element("Project");
        if (serverCoreProject != null)
        {
            var firstPackageReferenceItemGroup = serverCoreProject.Elements("ItemGroup")?.Where(ig => ig.Element("PackageReference") != null)?.FirstOrDefault();
            if (firstPackageReferenceItemGroup != null)
            {
                var firstUsingItemGroup = serverCoreProject.Elements("ItemGroup")?.Where(ig => ig.Element("Using") != null)?.FirstOrDefault();
                var firstEmbeddedResourceItemGroup = serverCoreProject.Elements("ItemGroup")?.Where(ig => ig.Element("EmbeddedResource") != null)?.FirstOrDefault();

                if (firstUsingItemGroup == null)
                {
                    firstUsingItemGroup = new XElement("ItemGroup");
                    firstPackageReferenceItemGroup.AddBeforeSelf(firstUsingItemGroup);
                }
                if (firstEmbeddedResourceItemGroup == null)
                {
                    firstEmbeddedResourceItemGroup = new XElement("ItemGroup");
                    firstPackageReferenceItemGroup.AddAfterSelf(firstEmbeddedResourceItemGroup);
                }
                serverCoreProject.Attribute("Sdk")?.SetValue("Microsoft.NET.Sdk.Razor");

                var newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", "Microsoft.AspNetCore.Http");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", "Microsoft.Extensions.Hosting");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", "Microsoft.Extensions.Logging");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", "System.Net.Http.Json");
                firstUsingItemGroup.Add(newElement);

                newUsingsListForCore.ForEach(us => firstUsingItemGroup.Add(us));

                newPackageReferencesCore.ForEach(pr => firstPackageReferenceItemGroup.Add(pr));

                foreach (var item in serverApiXDoc.Descendants("EmbeddedResource"))
                {
                    firstEmbeddedResourceItemGroup.Add(item);
                }
                serverApiXDoc.Descendants("ItemGroup").Where(ig => ig.Elements("EmbeddedResource").Count() == ig.Elements().Count()).Remove();
            }
        }


        var serverApiProject = serverApiXDoc.Element("Project");
        if (serverApiProject != null)
        {
            var firstPackageReferenceItemGroup = serverApiProject.Elements("ItemGroup")?.Where(ig => ig.Element("PackageReference") != null)?.FirstOrDefault();
            if (firstPackageReferenceItemGroup != null)
            {
                var firstUsingItemGroup = serverApiProject.Elements("ItemGroup")?.Where(ig => ig.Element("Using") != null)?.FirstOrDefault();
                if (firstUsingItemGroup == null)
                {
                    firstUsingItemGroup = new XElement("ItemGroup");
                    firstPackageReferenceItemGroup.AddBeforeSelf(firstUsingItemGroup);
                }



                var newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Microsoft.Extensions.Options");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Microsoft.AspNetCore.Authorization");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Microsoft.AspNetCore.Identity");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Microsoft.EntityFrameworkCore");
                firstUsingItemGroup.Add(newElement);
                //newElement = new XElement("Using");
                //newElement.SetAttributeValue("Include", $"Microsoft.AspNetCore.OData.Query");
                //firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Microsoft.AspNetCore.Mvc");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Microsoft.Extensions.AI");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"Hangfire");
                firstUsingItemGroup.Add(newElement);
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"ZiggyCreatures.Caching.Fusion");
                firstUsingItemGroup.Add(newElement);



                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core");
                firstUsingItemGroup.Add(newElement);
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Infrastructure", "SignalR")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Infrastructure.SignalR");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Attachments")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Attachments");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Categories")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Categories");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Chatbot")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Chatbot");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Identity")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Identity");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Products")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Products");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "PushNotification")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.PushNotification");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Statistics")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Statistics");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Todo")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{_projectName}.Server.Core.Features.Todo");
                    firstUsingItemGroup.Add(newElement);
                }

                //newElement = new XElement("PackageReference");
                //newElement.SetAttributeValue("Include", $"Microsoft.EntityFrameworkCore");
                //firstPackageReferenceItemGroup.Add(newElement);
                //newElement = new XElement("PackageReference");
                //newElement.SetAttributeValue("Include", $"Microsoft.AspNetCore.OpenApi");
                //firstPackageReferenceItemGroup.Add(newElement);
            }
        }


        serverApiXDoc.SaveCsproj(serverApiCsprojPath);
        serverCoreXDoc.SaveCsproj(serverCoreCsprojPath);
    }

    private static bool MustMoveToServerCore(string includeValueAttribute)
    {
        return !includeValueAttribute.Contains(".HealthChecks.") &&
            !includeValueAttribute.Contains(".SignalR") &&
            !includeValueAttribute.StartsWith("Asp.Versioning") &&
            !includeValueAttribute.StartsWith("QRCoder") &&
            !includeValueAttribute.StartsWith("HtmlSanitizer") &&
            !includeValueAttribute.StartsWith("Humanizer") &&
            !includeValueAttribute.StartsWith("Magick.") &&
            !includeValueAttribute.StartsWith("Microsoft.AspNetCore.OData") &&
            !includeValueAttribute.StartsWith("Microsoft.AspNetCore.OpenApi") &&
            !includeValueAttribute.StartsWith("Microsoft.Extensions.AI.") &&
            !includeValueAttribute.StartsWith("Microsoft.SemanticKernel") &&
            !includeValueAttribute.StartsWith("Scalar.");
    }

    private static void MoveResources()
    {
        string resourcesToMove = "IdentityStrings.";
        string resourcesTargetBefore = "EmailStrings.";

        string sourceProject = $"{_projectName}.Core";
        string sourceResourcesDirectory = "Resources";
        string sourceProjectDirectory = Path.Combine(_bitPlatformProjectFolder, "src", $"{sourceProject}");

        string targetProject = $"{_projectName}.Server.Api";
        string targetResourcesDirectory = Path.Combine("Features", "Identity", "Resources");
        string targetProjectDirectory = Path.Combine(_bitPlatformProjectFolder, "src", "Server", $"{targetProject}");
        MoveOrRenameFiles(
            Path.Combine(sourceProjectDirectory, sourceResourcesDirectory, $"{resourcesToMove}*"),
            Path.Combine(targetProjectDirectory, targetResourcesDirectory)
        );
        try
        {
            string sourceResourcesProjectFile = Path.Combine(sourceProjectDirectory, $"{sourceProject}.csproj");
            var sourceXDoc = XDocument.Load(sourceResourcesProjectFile);
            var sourceItems = GetRessourcesItemsFromCsproj(sourceXDoc, Path.Combine(sourceResourcesDirectory, resourcesToMove));
            if (sourceItems.Count == 0)
            {
                _errors.Add($@"ItemGroup section not found in source csproj file (for moving resources files).");
                return;
            }

            string targetResourcesProjectFile = Path.Combine(targetProjectDirectory, $"{targetProject}.csproj");
            var targetXDoc = XDocument.Load(targetResourcesProjectFile);
            var targetItems = GetRessourcesItemsFromCsproj(targetXDoc, Path.Combine(targetResourcesDirectory, resourcesToMove));
            if (targetItems.Count > 0)
            {
                _errors.Add($@"ItemGroup section found in target csproj file (for moving resources files).");
                return;
            }
            var targetItemsBefore = GetRessourcesItemsFromCsproj(targetXDoc, Path.Combine(targetResourcesDirectory, resourcesTargetBefore));
            if (targetItemsBefore.Count == 0)
            {
                _errors.Add($@"ItemGroup section (for Email resources) not found in target csproj file (for moving resources files).");
                return;
            }

            foreach (var item in sourceItems)
            {
                var attributeWithResourcesPath = item.Attribute("Update");
                if (attributeWithResourcesPath != null)
                {
                    attributeWithResourcesPath.Value = attributeWithResourcesPath.Value.Replace(sourceResourcesDirectory, targetResourcesDirectory);
                }
                var elementWithResourcesPath = item.Element("LastGenOutput");
                if (elementWithResourcesPath != null)
                {
                    elementWithResourcesPath.Value = elementWithResourcesPath.Value.Replace(sourceResourcesDirectory, targetResourcesDirectory);
                }
                elementWithResourcesPath = item.Element("DependentUpon");
                if (elementWithResourcesPath != null)
                {
                    elementWithResourcesPath.Value = elementWithResourcesPath.Value.Replace(sourceResourcesDirectory, targetResourcesDirectory);
                }

                var elementWithNamespace = item.Element("StronglyTypedNamespace");
                if (elementWithNamespace != null)
                {
                    elementWithNamespace.Value = $"{targetProject}.{targetResourcesDirectory.Replace(Path.DirectorySeparatorChar, '.')}";
                }
            }

            targetItemsBefore.FirstOrDefault()?.AddBeforeSelf(sourceItems);
            targetXDoc.SaveCsproj(targetResourcesProjectFile);

            sourceItems.Remove();
            sourceXDoc.Descendants("ItemGroup").Where(ig => ig.IsEmpty).Remove();
            sourceXDoc.SaveCsproj(sourceResourcesProjectFile);
        }
        catch (Exception)
        {
            _errors.Add($@"Error in moving ItemGroup in csproj file (for moving resources files).");
        }
    }

    private static List<XElement> GetRessourcesItemsFromCsproj(XDocument sourceXDoc, string updateValue)
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

    private static List<string> MoveFilesRecursively(string sourcePath, string targetPath, bool deleteEmptyDirectory, string? excludeFilesPattern = null, string? excludeDirectory = null)
    {
        var sourceDirectories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).Where(filename => excludeDirectory == null || !filename.Split([Path.DirectorySeparatorChar]).Contains(excludeDirectory));
        var sourceFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories).Where(filename => (string.IsNullOrEmpty(excludeFilesPattern) || !filename.Contains(excludeFilesPattern)) && (excludeDirectory == null || !filename.Split([Path.DirectorySeparatorChar]).Contains(excludeDirectory)));
        //Now Create all of the directories
        foreach (string dirPath in sourceDirectories)
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        List<string> movedFiles = new();
        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in sourceFiles)
        {
            var destPath = newPath.Replace(sourcePath, targetPath);
            File.Move(newPath, destPath, true);
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

    static void MoveOrRenameFiles(
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
    
    private static async Task ReplaceForwardedPortsAndUserSecretsAsync(int[]? bitPlatformForwardPorts, string bitPlatformUserSecrets, int[]? sourceForwardPorts, string sourceUserSecrets)
    {
        string launchSettingsFilename = "launchSettings.json";
        string csprojFilename = $"{_projectName}.Server.*.csproj";

        if (!string.Equals(sourceUserSecrets, bitPlatformUserSecrets, StringComparison.OrdinalIgnoreCase))
        {
            await ReplaceTextAsync(bitPlatformUserSecrets, sourceUserSecrets, false, true, [Path.Combine(_bitPlatformProjectFolder, "src")], [launchSettingsFilename, csprojFilename], expectedReplacements: 5);
        }
        if (sourceForwardPorts?.Length != 6 || bitPlatformForwardPorts?.Length != 6)
        {
            _errors.Add($@"Forward ports error.");
            return;
        }

        int?[][] expectedReplacements = [
            [1, null, 0, 0, 1 ],
            [1, null, 0, 0, 1 ],
            [1, null, 0, 0, 1 ],
            [1, null, 0, 0, 1 ],
            [0, null, 0, 0, 0 ],
            [1, null, 1, 1, 6 ]
            ];
        for (int i = 0; i < 6; i++)
        {
            if (sourceForwardPorts[i] != bitPlatformForwardPorts[i])
            {
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, ".devcontainer")], ["devcontainer.json"],
                    expectedReplacements: expectedReplacements[i][0]);
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                //await ReplaceTextAsync($":{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, false,
                [Path.Combine(_bitPlatformProjectFolder, ".docs")], ["*.md"],
                    expectedReplacements: expectedReplacements[i][1]);
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, "src", "Server", $"{_projectName}.Server.AppHost")], ["Program.cs"],
                    expectedReplacements: expectedReplacements[i][2]);
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, "src", "Client", $"{_projectName}.Client.Core")], ["appsettings.json"],
                    expectedReplacements: expectedReplacements[i][3]);

                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, "src")], [launchSettingsFilename],
                    expectedReplacements: expectedReplacements[i][4]);
            }
        }
    }

    private static string ReadUserSecrets(string projectPath)
    {
        string? userSecretValue = null;
        string userSecretsFilename = Path.Combine("src", "Server", $"{_projectName}.Server.Api", $"{_projectName}.Server.Api.csproj");
        try
        {
            var xdoc = XDocument.Load(Path.Combine(projectPath, userSecretsFilename));
            userSecretValue = xdoc.Document?.Element("Project")?.Elements("PropertyGroup")?.Select(el => el.Element("UserSecretsId")?.Value).Where(val => val != null).FirstOrDefault();
        }
        catch(Exception)
        {
            _errors.Add($@"Error reading the ""{userSecretsFilename}"" project file.");
        }
        if (string.IsNullOrWhiteSpace(userSecretValue))
        {
            _errors.Add($@"The UserSecretsId value is missing in the ""{_projectName}.Server.Api"" project file.");
            userSecretValue = "";
        }
        return userSecretValue;
    }
    private static int[]? ReadForwardPorts(string projectPath)
    {
        int[]? forwardPorts = null;
        string devContainerFilename = Path.Combine(".devcontainer", "devcontainer.json");
        try
        {
            string devContainerJson = File.ReadAllText(Path.Combine(projectPath, devContainerFilename));
            var jsonDocument = JsonSerializer.Deserialize<JsonDocument>(devContainerJson);
            forwardPorts = jsonDocument?.RootElement.GetProperty("forwardPorts").EnumerateArray().Select(elem => elem.GetInt32()).ToArray();
            if (forwardPorts == null || forwardPorts.Length != 6)
            {
                _errors.Add($@"The ""forwardPorts"" has not 6 ports in the JSON file ""{devContainerFilename}"".");
            }
        }
        catch (Exception)
        {
            _errors.Add($@"Error in the JSON file ""{devContainerFilename}"".");
        }
        return forwardPorts;
    }

    private static async Task RenameServerSharedProjectAsync(bool matchCase = false)
    {
        if (!MoveOrRenameDirectory(Path.Combine("src", "Server", $"{_projectName}.Server.Shared"),
                Path.Combine("src", "Server", $"{_projectName}.Server.Core")) ||
            !MoveOrRenameFile(Path.Combine("src", "Server", $"{_projectName}.Server.Core", $"{_projectName}.Server.Shared.csproj"),
                Path.Combine("src", "Server", $"{_projectName}.Server.Core", $"{_projectName}.Server.Core.csproj"))
            )
        {
            _errors.Add($@"Aborted: the project ""{_projectName}.Server.Shared"" must exist.");
            return;
        }

        await ReplaceTextAsync($@"{_projectName}.Server.Shared", $@"{_projectName}.Server.Core", matchCase, expectedReplacements: 102);
    }
    private static async Task RenameSharedProjectAsync(bool matchCase = false)
    {

        if (!MoveOrRenameDirectory(Path.Combine("src", "Shared"), Path.Combine("src", $"{_projectName}.Core")) ||
            !MoveOrRenameFile(Path.Combine("src", $"{_projectName}.Core", $"{_projectName}.Shared.csproj"),
                Path.Combine("src", $"{_projectName}.Core", $"{_projectName}.Core.csproj"))
            )
        {
            _errors.Add($@"Aborted: the project ""{_projectName}.Shared"" must exist.");
            return;
        }

        await ReplaceTextAsync($@"Shared/{_projectName}.Shared", $@"{_projectName}.Core/{_projectName}.Core", matchCase, expectedReplacements: 20);
        await ReplaceTextAsync($@"Shared\{_projectName}.Shared", $@"{_projectName}.Core\{_projectName}.Core", matchCase, expectedReplacements: 3);
        await ReplaceTextAsync($@"Shared\\{_projectName}.Shared", $@"{_projectName}.Core\\{_projectName}.Core", matchCase, expectedReplacements: 56);

        await ReplaceTextAsync($@"{_projectName}.Shared", $@"{_projectName}.Core", matchCase, expectedReplacements: 339);

        await ReplaceTextAsync($@"Shared/", $@"{_projectName}.Core/", matchCase, expectedReplacements: 54);
        await ReplaceTextAsync($@"Shared`", $@"{_projectName}.Core`", matchCase, expectedReplacements: 3);
        await ReplaceTextAsync($@"Shared""", $@"{_projectName}.Core""", matchCase, expectedReplacements: 1);

        await ReplaceTextAsync($@"Shared project to", $@"shared project (`{_projectName}.Core`) to", matchCase, expectedReplacements: 1);
    }
    private static async Task<int> ReplaceTextAsync(string searchText, string replaceText, bool matchCase = false, bool matchWholeWord = false, string[]? directories = null, string[]? filters = null, int? expectedReplacements = null, string[]? excludeDirectories = null)
    {
        if (directories == null || directories.Length == 0)
        {
            directories = [_bitPlatformProjectFolder];
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
        if (_useExpectedReplacements && expectedReplacements != null && (expectedReplacements >= 0 ? listFiles.Count != expectedReplacements : listFiles.Count < -expectedReplacements))
        {
            _errors.Add($@"Replacements for ""{searchText}"" ({string.Join('|', filters)}): {listFiles.Count} / Expected: {(expectedReplacements >= 0 ? expectedReplacements : $">= {-expectedReplacements}")}");
        }
        return listFiles.Count;
    }
    private static bool MoveOrRenameDirectory(string oldRelativePath, string newRelativePath)
    {
        var oldFullPath = Path.Combine(_bitPlatformProjectFolder, oldRelativePath);
        var newFullPath = Path.Combine(_bitPlatformProjectFolder, newRelativePath);
        if (Directory.Exists(newFullPath))
        {
            _errors.Add($@"The directory ""{newRelativePath}"" already exists.");
            return true;
        }

        try
        {
            Directory.Move(oldFullPath, newFullPath);

        }
        catch (Exception)
        {
            _errors.Add($@"The directory ""{oldRelativePath}"" cannot be moved (or renamed).");
            return false;
        }
        return true;
    }
    private static bool MoveOrRenameFile(string oldRelativePath, string newRelativePath)
    {
        var oldFullPath = Path.Combine(_bitPlatformProjectFolder, oldRelativePath);
        var newFullPath = Path.Combine(_bitPlatformProjectFolder, newRelativePath);
        if (File.Exists(newFullPath))
        {
            _errors.Add($@"The file ""{newRelativePath}"" already exists.");
            return true;
        }
        try
        {
            File.Move(oldFullPath, newFullPath);
        }
        catch (Exception)
        {
            _errors.Add($@"The file ""{oldRelativePath}"" cannot be moved (or renamed).");
            return false;
        }
        return true;
    }
}
