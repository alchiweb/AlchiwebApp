using System.Transactions;
using System.Xml.Linq;
using AlchiwebApp.Console.Core.Services;
using Rejigs;

internal class Program
{
    static string _bitPlatformProjectFolder = "";
    static string _sourceProjectFolder = "";
    static string _projectName = "";
    static readonly FileSearchService _searchService = new();
    static List<string> _errors = [];
    static int[]? _bitPlatformForwardPorts;
    static int[]? _sourceForwardPorts;
    static string _bitPlatformUserSecrets = "";
    static string _sourceUserSecrets = "";

    private static async Task Main(string[] args)
    {
        _bitPlatformProjectFolder = Directory.GetCurrentDirectory();
        _projectName = Path.GetFileName(_bitPlatformProjectFolder);
        _sourceProjectFolder = Path.Combine(_bitPlatformProjectFolder, @"..\..");
        Console.WriteLine($"AlchiwebApp / Add To BitPlatform");
        Console.WriteLine($"Project name: {_projectName} / Project directory: {_bitPlatformProjectFolder}");

        _sourceForwardPorts = ReadForwardPorts(_sourceProjectFolder);
        _bitPlatformForwardPorts = ReadForwardPorts(_bitPlatformProjectFolder);
        _sourceUserSecrets = ReadUserSecrets(_sourceProjectFolder);
        _bitPlatformUserSecrets = ReadUserSecrets(_bitPlatformProjectFolder);

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

                await ReplaceForwardedPortsAndUserSecretsAsync();

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
        MoveOrRenameFile(
            Path.Combine(_bitPlatformProjectFolder, $@"src\{_projectName}.Core\Infrastructure\Extensions\ActivitySourceExtensions.cs"),
            Path.Combine(_bitPlatformProjectFolder, $@"src\Server\{_projectName}.Server.Core\Infrastructure\Extensions\ActivitySourceExtensions.cs")
            );
        MoveOrRenameFile(
            Path.Combine(_bitPlatformProjectFolder, $@"src\{_projectName}.Core\Infrastructure\Extensions\MeterExtensions.cs"),
            Path.Combine(_bitPlatformProjectFolder, $@"src\Server\{_projectName}.Server.Core\Infrastructure\Extensions\MeterExtensions.cs")
        );

        MoveResources();
    }

    private static void MoveResources()
    {
        string resourcesToMove = "IdentityStrings.";
        string resourcesTargetBefore = "EmailStrings.";

        string sourceProject = $@"{_projectName}.Core";
        string sourceResourcesDirectory = @"Resources";
        string sourceProjectDirectory = Path.Combine(_bitPlatformProjectFolder, @"src", $"{sourceProject}");

        string targetProject = $@"{_projectName}.Server.Api";
        string targetResourcesDirectory = @"Features\Identity\Resources";
        string targetProjectDirectory = Path.Combine(_bitPlatformProjectFolder, @"src\Server", $"{targetProject}");
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
                    elementWithNamespace.Value = $"{targetProject}.{targetResourcesDirectory.Replace('\\', '.')}";
                }
            }

            targetItemsBefore.FirstOrDefault()?.AddBeforeSelf(sourceItems);
            targetXDoc.Save(targetResourcesProjectFile);

            sourceItems.Remove();
            sourceXDoc.Descendants("ItemGroup").Where(ig => ig.IsEmpty).Remove();
            sourceXDoc.Save(sourceResourcesProjectFile);
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
    
    private static async Task ReplaceForwardedPortsAndUserSecretsAsync()
    {
        string launchSettingsFilename = @"launchSettings.json";
        string csprojFilename = $@"{_projectName}.Server.*.csproj";

        if (!string.Equals(_sourceUserSecrets, _bitPlatformUserSecrets, StringComparison.OrdinalIgnoreCase))
        {
            await ReplaceTextAsync(_bitPlatformUserSecrets, _sourceUserSecrets, false, true, [Path.Combine(_bitPlatformProjectFolder, $@"src")], [launchSettingsFilename, csprojFilename], expectedReplacements: 5);
        }
        if (_sourceForwardPorts?.Length != 6 || _bitPlatformForwardPorts?.Length != 6)
        {
            _errors.Add($@"Forward ports error.");
            return;
        }

        int[][] expectedReplacements = [
            [1, 1, 0, 0, 1 ],
            [1, 0, 0, 0, 1 ],
            [1, 0, 0, 0, 1 ],
            [1, 0, 0, 0, 1 ],
            [0, 0, 0, 0, 0 ],
            [1, 0, 1, 1, 6 ]
            ];
        for (int i = 0; i < 6; i++)
        {
            if (_sourceForwardPorts[i] != _bitPlatformForwardPorts[i])
            {
                await ReplaceTextAsync(_bitPlatformForwardPorts[i].ToString(), _sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, $@".devcontainer")], ["devcontainer.json"],
                    expectedReplacements: expectedReplacements[i][0]);
                await ReplaceTextAsync(_bitPlatformForwardPorts[i].ToString(), _sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, $@".docs")], ["*.md"],
                    expectedReplacements: expectedReplacements[i][1]);
                await ReplaceTextAsync(_bitPlatformForwardPorts[i].ToString(), _sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, $@"src\Server\{_projectName}.Server.AppHost")], ["Program.cs"],
                    expectedReplacements: expectedReplacements[i][2]);
                await ReplaceTextAsync(_bitPlatformForwardPorts[i].ToString(), _sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, $@"src\Client\{_projectName}.Client.Core")], ["appsettings.json"],
                    expectedReplacements: expectedReplacements[i][3]);

                await ReplaceTextAsync(_bitPlatformForwardPorts[i].ToString(), _sourceForwardPorts[i].ToString(), false, true,
                    [Path.Combine(_bitPlatformProjectFolder, $@"src")], [launchSettingsFilename],
                    expectedReplacements: expectedReplacements[i][4]);
            }
        }
    }

    private static string ReadUserSecrets(string projectPath)
    {
        string? userSecretValue = null;
        string userSecretsFilename = $@"src\Server\{_projectName}.Server.AppHost\{_projectName}.Server.AppHost.csproj";
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
            _errors.Add($@"The UserSecretsId value is missing in the ""{_projectName}.Server.AppHost"" project file.");
            userSecretValue = "";
        }
        return userSecretValue;
    }
    private static int[]? ReadForwardPorts(string projectPath)
    {
        int[]? forwardPorts = null;
        string devContainerFilename = @".devcontainer\devcontainer.json";
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
        if (!MoveOrRenameDirectory($@"src\Server\{_projectName}.Server.Shared", $@"src\Server\{_projectName}.Server.Core") ||
            !MoveOrRenameFile($@"src\Server\{_projectName}.Server.Core\{_projectName}.Server.Shared.csproj", $@"src\Server\{_projectName}.Server.Core\{_projectName}.Server.Core.csproj")
            )
        {
            _errors.Add($@"Aborted: the project ""{_projectName}.Server.Shared"" must exist.");
            return;
        }

        await ReplaceTextAsync($@"{_projectName}.Server.Shared", $@"{_projectName}.Server.Core", matchCase, expectedReplacements: 102);
    }
    private static async Task RenameSharedProjectAsync(bool matchCase = false)
    {

        if (!MoveOrRenameDirectory($@"src\Shared", $@"src\{_projectName}.Core") ||
            !MoveOrRenameFile($@"src\{_projectName}.Core\{_projectName}.Shared.csproj", $@"src\{_projectName}.Core\{_projectName}.Core.csproj")
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
        if (expectedReplacements != null && (expectedReplacements >= 0 ? listFiles.Count != expectedReplacements : listFiles.Count < -expectedReplacements))
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
