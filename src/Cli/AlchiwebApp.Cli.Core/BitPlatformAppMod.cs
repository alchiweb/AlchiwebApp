using System.Xml.Linq;
using AlchiwebApp.Cli.Core.Services;

namespace AlchiwebApp.Cli.Core;

public partial class BitPlatformAppMod : BitPlatformApp
{
    private readonly FileSearchService _searchService;

    public BitPlatformAppMod(string bitPlatformProjectFolderPath, string sourceProjectFolder, bool useExpectedReplacements, FileSearchService searchService)
        : base(bitPlatformProjectFolderPath, sourceProjectFolder, useExpectedReplacements, searchService)
    {
        _searchService = searchService;
    }

    public async Task ModifyBitPlatformProject()
    {
        Console.WriteLine($"Project name: {ProjectName} / Project directory: {BitPlatformProjectFolder}");
        int[]? bitPlatformForwardPorts = null;
        int[]? sourceForwardPorts = null;
        string bitPlatformUserSecrets = "";
        string sourceUserSecrets = "";

        bool hasSourceBitPlatformProject = !string.Equals(BitPlatformProjectFolder, SourceProjectFolder);

        if (hasSourceBitPlatformProject)
        {
            // Reading ForwardedPorts and UserSecrets
            sourceForwardPorts = ReadForwardPorts(SourceProjectFolder);
            bitPlatformForwardPorts = ReadForwardPorts(BitPlatformProjectFolder);
            sourceUserSecrets = ReadUserSecrets(SourceProjectFolder);
            bitPlatformUserSecrets = ReadUserSecrets(BitPlatformProjectFolder);
        }
        if (Errors.Count > 0)
        {
            Errors.Add($@"Aborted: critical error.");
        }
        else
        {
            try
            {
                // Renaming Shared projects to Core projects
                await RenameServerSharedProjectAsync(true);
                await RenameSharedProjectAsync(true);

                if (hasSourceBitPlatformProject)
                {
                    // Replacing ForwardedPorts and UserSecrets
                    await ReplaceForwardedPortsAndUserSecretsAsync(bitPlatformForwardPorts, bitPlatformUserSecrets, sourceForwardPorts, sourceUserSecrets);
                }
                // Moving files to Server.Core project
                await MoveFilesToServerCoreProjectAsync();
            }
            catch (Exception ex)
            {
                Errors.Add($@"Aborted: critical error ({ex.Message}).");
            }
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

    #region Renaming Shared projects to Core projects
    private async Task RenameServerSharedProjectAsync(bool matchCase = false)
    {
        if (!MoveOrRenameDirectory(Path.Combine("src", "Server", $"{ProjectName}.Server.Shared"),
                Path.Combine("src", "Server", $"{ProjectName}.Server.Core")) ||
            !MoveOrRenameFile(Path.Combine("src", "Server", $"{ProjectName}.Server.Core", $"{ProjectName}.Server.Shared.csproj"),
                Path.Combine("src", "Server", $"{ProjectName}.Server.Core", $"{ProjectName}.Server.Core.csproj"))
            )
        {
            Errors.Add($@"Aborted: the project ""{ProjectName}.Server.Shared"" must exist.");
            return;
        }

        await ReplaceTextAsync($@"{ProjectName}.Server.Shared", $@"{ProjectName}.Server.Core", matchCase, expectedReplacements: 102);
    }
    private async Task RenameSharedProjectAsync(bool matchCase = false)
    {

        if (!MoveOrRenameDirectory(Path.Combine("src", "Shared"), Path.Combine("src", $"{ProjectName}.Core")) ||
            !MoveOrRenameFile(Path.Combine("src", $"{ProjectName}.Core", $"{ProjectName}.Shared.csproj"),
                Path.Combine("src", $"{ProjectName}.Core", $"{ProjectName}.Core.csproj"))
            )
        {
            Errors.Add($@"Aborted: the project ""{ProjectName}.Shared"" must exist.");
            return;
        }

        await ReplaceTextAsync($@"Shared/{ProjectName}.Shared", $@"{ProjectName}.Core/{ProjectName}.Core", matchCase, expectedReplacements: 20);
        await ReplaceTextAsync($@"Shared\{ProjectName}.Shared", $@"{ProjectName}.Core\{ProjectName}.Core", matchCase, expectedReplacements: 3);
        await ReplaceTextAsync($@"Shared\\{ProjectName}.Shared", $@"{ProjectName}.Core\\{ProjectName}.Core", matchCase, expectedReplacements: 56);

        await ReplaceTextAsync($@"{ProjectName}.Shared", $@"{ProjectName}.Core", matchCase, expectedReplacements: 339);

        await ReplaceTextAsync($@"Shared/", $@"{ProjectName}.Core/", matchCase, expectedReplacements: 54);
        await ReplaceTextAsync($@"Shared`", $@"{ProjectName}.Core`", matchCase, expectedReplacements: 3);
        await ReplaceTextAsync($@"Shared""", $@"{ProjectName}.Core""", matchCase, expectedReplacements: 1);

        await ReplaceTextAsync($@"Shared project to", $@"shared project (`{ProjectName}.Core`) to", matchCase, expectedReplacements: 1);
    }
    #endregion

    #region Reading and replacing ForwardedPorts and UserSecrets
    private async Task ReplaceForwardedPortsAndUserSecretsAsync(int[]? bitPlatformForwardPorts, string bitPlatformUserSecrets, int[]? sourceForwardPorts, string sourceUserSecrets)
    {
        string launchSettingsFilename = "launchSettings.json";
        string csprojFilename = $"{ProjectName}.Server.*.csproj";

        if (!string.Equals(sourceUserSecrets, bitPlatformUserSecrets, StringComparison.OrdinalIgnoreCase))
        {
            await ReplaceTextAsync(bitPlatformUserSecrets, sourceUserSecrets, false, true, [Path.Combine(BitPlatformProjectFolder, "src")], [launchSettingsFilename, csprojFilename], expectedReplacements: 5);
        }
        if (sourceForwardPorts?.Length != 6 || bitPlatformForwardPorts?.Length != 6)
        {
            Errors.Add($@"Forward ports error.");
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
                    [Path.Combine(BitPlatformProjectFolder, ".devcontainer")], ["devcontainer.json"],
                    expectedReplacements: expectedReplacements[i][0]);
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                //await ReplaceTextAsync($":{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, false,
                [Path.Combine(BitPlatformProjectFolder, ".docs")], ["*.md"],
                    expectedReplacements: expectedReplacements[i][1]);
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(BitPlatformProjectFolder, "src", "Server", $"{ProjectName}.Server.AppHost")], ["Program.cs"],
                    expectedReplacements: expectedReplacements[i][2]);
                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(BitPlatformProjectFolder, "src", "Client", $"{ProjectName}.Client.Core")], ["appsettings.json"],
                    expectedReplacements: expectedReplacements[i][3]);

                await ReplaceTextAsync($"{bitPlatformForwardPorts[i]}", sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(BitPlatformProjectFolder, "src")], [launchSettingsFilename],
                    expectedReplacements: expectedReplacements[i][4]);
            }
        }
    }
    private string ReadUserSecrets(string projectPath)
    {
        string? userSecretValue = null;
        string userSecretsFilename = Path.Combine("src", "Server", $"{ProjectName}.Server.Api", $"{ProjectName}.Server.Api.csproj");
        try
        {
            var xdoc = XDocument.Load(Path.Combine(projectPath, userSecretsFilename));
            userSecretValue = xdoc.Document?.Element("Project")?.Elements("PropertyGroup")?.Select(el => el.Element("UserSecretsId")?.Value).Where(val => val != null).FirstOrDefault();
        }
        catch (Exception)
        {
            Errors.Add($@"Error reading the ""{userSecretsFilename}"" project file.");
        }
        if (string.IsNullOrWhiteSpace(userSecretValue))
        {
            Errors.Add($@"The UserSecretsId value is missing in the ""{ProjectName}.Server.Api"" project file.");
            userSecretValue = "";
        }
        return userSecretValue;
    }
    private int[]? ReadForwardPorts(string projectPath)
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
                Errors.Add($@"The ""forwardPorts"" has not 6 ports in the JSON file ""{devContainerFilename}"".");
            }
        }
        catch (Exception)
        {
            Errors.Add($@"Error in the JSON file ""{devContainerFilename}"".");
        }
        return forwardPorts;
    }
    #endregion

    #region Moving files to Server.Core project
    private async Task MoveFilesToServerCoreProjectAsync()
    {
        string sourceCorePath = Path.Combine(BitPlatformProjectFolder, "src", $"{ProjectName}.Core");
        string sourceServerCorePath = Path.Combine(BitPlatformProjectFolder, "src", "Server", $"{ProjectName}.Server.Core");
        string sourceServerApiPath = Path.Combine(BitPlatformProjectFolder, "src", "Server", $"{ProjectName}.Server.Api");
        string sourceServerWebPath = Path.Combine(BitPlatformProjectFolder, "src", "Server", $"{ProjectName}.Server.Web");
        string sourceTestsPath = Path.Combine(BitPlatformProjectFolder, "src", "Tests");

        // Move some resources from Core project to Server.Api project (before moving to Server.Core)
        MoveResources();

        // Move ServerApiSettings.cs (in Server.Api project) to ServerCoreSettings.cs (in Server.Core project)
        MoveOrRenameFile(
            Path.Combine(sourceServerApiPath, "ServerApiSettings.cs"),
            Path.Combine(sourceServerCorePath, "ServerCoreSettings.cs")
            );
        // Replace "ServerApiSettings" with "ServerCoreSettings" in all files in Server.Api and Server.Core projects
        await ReplaceTextAsync("ServerApiSettings", "ServerCoreSettings", true, true, [
            sourceServerApiPath,
            sourceServerCorePath
            ]);

        // Move all files in Features from Server.Api project to Server.Core project
        MoveFilesRecursively(
            Path.Combine(sourceServerApiPath, "Features"),
            Path.Combine(sourceServerCorePath, "Features"),
            true,
            "Controller."
            );

        // Move some extensions files (in /Instrastructure/Extension) from Core project to Server.Core project
        MoveOrRenameFile(
            Path.Combine(sourceCorePath, "Infrastructure", "Extensions", "ActivitySourceExtensions.cs"),
            Path.Combine(sourceServerCorePath, "Infrastructure", "Extensions", "ActivitySourceExtensions.cs")
            );
        MoveOrRenameFile(
            Path.Combine(sourceCorePath, "Infrastructure", "Extensions", "MeterExtensions.cs"),
            Path.Combine(sourceServerCorePath, "Infrastructure", "Extensions", "MeterExtensions.cs")
        );

        // Rename some extensions files that are duplicated in Server.Api and Server.Core: *.cs -> *.FromApi.cs
        var extensionsCsFile1 = Path.Combine("Infrastructure", "Extensions", "HttpContextExtensions");
        var extensionsCsFile2 = Path.Combine("Infrastructure", "Extensions", "HttpRequestExtensions");
        MoveOrRenameFile(
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile1}.cs"),
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile1}.FromApi.cs")
            );
        MoveOrRenameFile(
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile2}.cs"),
            Path.Combine(sourceServerApiPath, $"{extensionsCsFile2}.FromApi.cs")
            );

        // Move all files in Infrastructure from Server.Api project to Server.Core project
        MoveFilesRecursively(
            Path.Combine(sourceServerApiPath, "Infrastructure"),
            Path.Combine(sourceServerCorePath, "Infrastructure"),
            true,
            excludeDirectory: "Controllers"
            );

        // Change "static class" to "static partial class" for the duplicated extensions files (that were in Server.Api and Server.Core projects)
        string[] arrayExtensionsFiles = [
            Path.Combine(sourceServerCorePath, $"{extensionsCsFile1}.cs"),
            Path.Combine(sourceServerCorePath, $"{extensionsCsFile1}.FromApi.cs"),
            Path.Combine(sourceServerCorePath, $"{extensionsCsFile2}.cs"),
            Path.Combine(sourceServerCorePath, $"{extensionsCsFile2}.FromApi.cs"),
            Path.Combine(sourceServerCorePath, "Infrastructure", "Extensions", "WebApplicationBuilderExtensions.cs"),
            ];

        await ReplacePartialClassAsync(arrayExtensionsFiles, true, "static");
        arrayExtensionsFiles = [
            Path.Combine(sourceCorePath, "Infrastructure", "Services", $"AppFeatures.cs"),
            Path.Combine(sourceCorePath, "Infrastructure", "Services", $"AppRoles.cs")
            ];
        await ReplacePartialClassAsync(arrayExtensionsFiles);

        // For Core project
        RemoveUnusedPackages(sourceCorePath);

        // Modify csproj files (usings, package references, embedded resources)
        ChangeCsprojFiles(sourceServerCorePath, sourceServerApiPath);

        // Replace references to Server.Api with Server.Core
        await ReplaceTextAsync($@"{ProjectName}.Server.Api", $@"{ProjectName}.Server.Core", true, true,
            [sourceServerCorePath, sourceTestsPath]);
        await ReplaceTextAsync($@"using {ProjectName}.Server.Api.Infrastructure", $@"using {ProjectName}.Server.Core.Infrastructure", true, true,
            [sourceServerApiPath, sourceServerWebPath]);
        await ReplaceTextAsync($@"using {ProjectName}.Server.Api.Features", $@"using {ProjectName}.Server.Core.Features", true, true,
            [sourceServerApiPath, sourceServerWebPath]);
        //await _searchService.ReplaceInFilesAsync($"(\\n\\n^namespace ({ProjectName}\\.Server\\.)Api(\\.Features\\..*);$)", "\\nusing $2Core$3;$1",
        //    [new SearchResult() { FilePath = Path.Combine(sourceServerApiPath, "Features", "*.*")}],
        //    useRegex: true);
        //await _searchService.ReplaceInFilesAsync($"(\\n\\n^namespace ({ProjectName}\\.Server\\.)Api(\\.Infrastruture\\..*);$)", "\\nusing $2Core$3;$1",
        //    [new SearchResult() { FilePath = Path.Combine(sourceServerApiPath, "Infrastructure", "*.*") }],
        //    useRegex: true);
        //await _searchService.ReplaceInFilesAsync($"(\\n\\n^namespace ({ProjectName}\\.Server\\.)Api;$)", "\\nusing $2Core;$1",
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

        //// Clean Test project
        //try
        //{
        //    Directory.Delete(Path.Combine(sourceTestsPath, "bin"), true);
        //    Directory.Delete(Path.Combine(sourceTestsPath, "obj"), true);
        //}
        //catch (Exception) { }
    }


    private void ChangeCsprojFiles(string sourceServerCorePath, string sourceServerApiPath)
    {
        var serverApiCsprojPath = Path.Combine(sourceServerApiPath, $"{ProjectName}.Server.Api.csproj");
        var serverCoreCsprojPath = Path.Combine(sourceServerCorePath, $"{ProjectName}.Server.Core.csproj");
        var serverApiXDoc = XDocument.Load(serverApiCsprojPath);
        var serverCoreXDoc = XDocument.Load(serverCoreCsprojPath);

        List<XElement> usingsListForApi = serverApiXDoc.Descendants("Using").ToList();
        //List<XElement> newUsingsListForCore = serverCoreXDoc.Descendants("Using").ToList();
        List<string> usingsTextListForCore = serverCoreXDoc.Descendants("Using")
            .Select(elt => elt?.Attribute("Include")?.Value)
            .Where(val => !string.IsNullOrEmpty(val)).ToList();
        string controllersUsing = $"{ProjectName}.Server.Api.Infrastructure.Controllers";
        List<XElement> controllersUsings = usingsListForApi.Where(u => u.Attribute("Include")?.Value.StartsWith(controllersUsing) == true).ToList();

        foreach (var us in usingsListForApi.Except(controllersUsings))
        {
            var includeAttribute = us.Attribute("Include");
            if (includeAttribute?.Value != null &&
                    (
                        includeAttribute.Value.StartsWith($"{ProjectName}.Server.Api.Features") ||
                        includeAttribute.Value.StartsWith($"{ProjectName}.Server.Api.Infrastructure")
                    )
                )
            {
                includeAttribute.Value = includeAttribute.Value.Replace($"{ProjectName}.Server.Api", $"{ProjectName}.Server.Core");
            }
        }

        var newUsingsListForCore = new List<XElement>();
        newUsingsListForCore = usingsListForApi.Distinct().Except(controllersUsings).Where(us =>
        {
            var includeValueAttribute = us.Attribute("Include")?.Value;
            if (includeValueAttribute == null || usingsTextListForCore.Contains(includeValueAttribute))
                return false;
            var moveToCore = MustMoveToServerCore(includeValueAttribute);

            if (moveToCore && !includeValueAttribute.StartsWith($"{ProjectName}.Server.Core"))
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

                MoveEmbeddedResources(serverApiXDoc, firstEmbeddedResourceItemGroup);
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


                // TODO: specific -> generic
                newElement = new XElement("Using");
                newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core");
                firstUsingItemGroup.Add(newElement);
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Infrastructure", "SignalR")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Infrastructure.SignalR");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Attachments")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Attachments");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Categories")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Categories");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Chatbot")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Chatbot");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Identity")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Identity");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Products")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Products");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "PushNotification")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.PushNotification");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Statistics")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Statistics");
                    firstUsingItemGroup.Add(newElement);
                }
                if (Directory.Exists(Path.Combine(sourceServerCorePath, "Features", "Todo")))
                {
                    newElement = new XElement("Using");
                    newElement.SetAttributeValue("Include", $"{ProjectName}.Server.Core.Features.Todo");
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


        serverApiXDoc.SaveXmlFile(serverApiCsprojPath);
        serverCoreXDoc.SaveXmlFile(serverCoreCsprojPath);
    }

    private void RemoveUnusedPackages(string sourceCorePath)
    {
        var coreCsprojPath = Path.Combine(sourceCorePath, $"{ProjectName}.Core.csproj");
        var coreXDoc = XDocument.Load(coreCsprojPath);
        string[] packageReferenceItemsToRemove = {
            "Microsoft.AspNetCore.Authorization",
            "Microsoft.Extensions.DependencyInjection.Abstractions",
            "Microsoft.Extensions.Options.ConfigurationExtensions"
        };
        coreXDoc.Descendants("PackageReference")
            .Where(elt => packageReferenceItemsToRemove.Contains(elt.Attribute("Include")?.Value))
            .Remove();
        coreXDoc.SaveXmlFile(coreCsprojPath);
    }

    /// <summary>
    /// Move EmbeddedRerource elements from serverApiXDoc to firstEmbeddedResourceItemGroup
    /// </summary>
    /// <param name="serverApiXDoc"></param>
    /// <param name="firstEmbeddedResourceItemGroup"></param>
    private void MoveEmbeddedResources(XDocument serverApiXDoc, XElement firstEmbeddedResourceItemGroup)
    {
        XElement[]? sourceElements = serverApiXDoc.Descendants("EmbeddedResource")?.ToArray();
        if (sourceElements == null || sourceElements.Count() == 0)
        {
            return;
        }
        foreach (var item in sourceElements)
        {
            firstEmbeddedResourceItemGroup.Add(item);
        }
        sourceElements.Remove();

        sourceElements = serverApiXDoc.Descendants("Content").Where(elt => string.Equals(elt.Attribute("Include")?.Value, @"..\..\..\Bit.ResxTranslator.json"))?.ToArray();
        if (sourceElements == null || sourceElements.Count() == 0 || firstEmbeddedResourceItemGroup?.Document == null)
        {
            return;
        }
        var itemGroup = AddItemGroup(firstEmbeddedResourceItemGroup.Document);
        if (itemGroup == null)
        {
            return;
        }
        foreach (var item in sourceElements)
        {
            itemGroup.Add(item);
        }
        sourceElements.Remove();
    }

    private bool MustMoveToServerCore(string includeValueAttribute)
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

    private void MoveResources()
    {
        string resourcesToMove = "IdentityStrings.";
        string resourcesTargetBefore = "EmailStrings.";

        string sourceProject = $"{ProjectName}.Core";
        string sourceResourcesDirectory = "Resources";
        string sourceProjectDirectory = Path.Combine(BitPlatformProjectFolder, "src", $"{sourceProject}");

        string targetProject = $"{ProjectName}.Server.Api";
        string targetResourcesDirectory = Path.Combine("Features", "Identity", "Resources");
        string targetProjectDirectory = Path.Combine(BitPlatformProjectFolder, "src", "Server", $"{targetProject}");
        MoveOrRenameFiles(
            Path.Combine(sourceProjectDirectory, sourceResourcesDirectory, $"{resourcesToMove}*"),
            Path.Combine(targetProjectDirectory, targetResourcesDirectory)
        );
        try
        {
            string sourceResourcesProjectFile = Path.Combine(sourceProjectDirectory, $"{sourceProject}.csproj");
            var sourceXDoc = XDocument.Load(sourceResourcesProjectFile);
            var sourceItems = GetResourcesItemsFromCsproj(sourceXDoc, Path.Combine(sourceResourcesDirectory, resourcesToMove));
            if (sourceItems.Count == 0)
            {
                Errors.Add($@"ItemGroup section not found in source csproj file (for moving resources files).");
                return;
            }

            string targetResourcesProjectFile = Path.Combine(targetProjectDirectory, $"{targetProject}.csproj");
            var targetXDoc = XDocument.Load(targetResourcesProjectFile);
            var targetItems = GetResourcesItemsFromCsproj(targetXDoc, Path.Combine(targetResourcesDirectory, resourcesToMove));
            if (targetItems.Count > 0)
            {
                Errors.Add($@"ItemGroup section found in target csproj file (for moving resources files).");
                return;
            }
            var targetItemsBefore = GetResourcesItemsFromCsproj(targetXDoc, Path.Combine(targetResourcesDirectory, resourcesTargetBefore));
            if (targetItemsBefore.Count == 0)
            {
                Errors.Add($@"ItemGroup section (for Email resources) not found in target csproj file (for moving resources files).");
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
            targetXDoc.SaveXmlFile(targetResourcesProjectFile);

            sourceItems.Remove();
            sourceXDoc.Descendants("ItemGroup").Where(ig => ig.IsEmpty).Remove();
            sourceXDoc.SaveXmlFile(sourceResourcesProjectFile);
        }
        catch (Exception)
        {
            Errors.Add($@"Error in moving ItemGroup in csproj file (for moving resources files).");
        }
    }

    #endregion

}
