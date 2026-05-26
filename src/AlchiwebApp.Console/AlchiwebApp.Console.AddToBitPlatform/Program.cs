using AlchiwebApp.Console.Core.Services;

internal class Program
{
    static string _projectFolder = "";
    static string _projectName = "";
    static readonly FileSearchService _searchService = new();
    static async Task Main(string[] args)
    {
        Console.WriteLine($"AlchiwebApp / Add To BitPlatform");
        _projectFolder = Directory.GetCurrentDirectory();
        _projectName = Path.GetFileName(_projectFolder);
        await RenameServerSharedProject();
        await RenameSharedProject();
        Console.WriteLine($"Project name: {_projectName}");
        Console.WriteLine($"Project directory: {_projectFolder}");
    }
    static async Task RenameServerSharedProject()
    {
        try
        {
            File.Move($@"{_projectFolder}\src\Server\{_projectName}.Server.Shared\{_projectName}.Server.Shared.csproj", $@"{_projectFolder}\src\Server\{_projectName}.Server.Shared\{_projectName}.Server.Core.csproj");
        }
        catch (Exception) { }
        try
        {
            Directory.Move($@"{_projectFolder}\src\Server\{_projectName}.Server.Shared", $@"{_projectFolder}\src\Server\{_projectName}.Server.Core");
        }
        catch (Exception) { }

        var searchPattern = $@"{_projectName}.Server.Shared";
        var listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Server.Core", listFiles);
    }
    static async Task RenameSharedProject()
    {
        try
        {
            File.Move($@"{_projectFolder}\src\Shared\{_projectName}.Shared.csproj", $@"{_projectFolder}\src\Shared\{_projectName}.Core.csproj");
        }
        catch (Exception) { }
        try
        {
            Directory.Move($@"{_projectFolder}\src\Shared", $@"{_projectFolder}\src\{_projectName}.Core");
        }
        catch (Exception) { }

        var searchPattern = $@"Shared/{_projectName}.Shared";
        var listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core/{_projectName}.Core", listFiles);

        searchPattern = searchPattern.Replace("/", @"\");
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core\{_projectName}.Core", listFiles);

        searchPattern = searchPattern.Replace(@"\", @"\\");
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core\\{_projectName}.Core", listFiles);

        searchPattern = searchPattern.Replace(@"Shared\\", @"");
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core", listFiles);

        searchPattern = $@"Shared/";
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core/", listFiles);

        searchPattern = $@"Shared`";
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core`", listFiles);

        searchPattern = $@"Shared""";
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"{_projectName}.Core""", listFiles);

        searchPattern = "Shared project to";
        listFiles = await _searchService.SearchFilesAsync([_projectFolder], ["*.*"], true, searchPattern);
        await _searchService.ReplaceInFilesAsync(searchPattern, $@"shared project (`{_projectName}.Core`) to", listFiles);
    }
}
