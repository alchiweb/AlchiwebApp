using System.Xml.Linq;
using AlchiwebApp.Cli.Core.Services;

namespace AlchiwebApp.Cli.Core;

public partial class BitPlatformAppModUpgrade : BitPlatformApp
{
    private readonly FileSearchService _searchService;

    public BitPlatformAppModUpgrade(string bitPlatformProjectFolderPath, string sourceProjectFolder, bool useExpectedReplacements, FileSearchService searchService)
        : base(bitPlatformProjectFolderPath, sourceProjectFolder, useExpectedReplacements, searchService)
    {
        _searchService = searchService;
    }

    public async Task AddAlchiwebApp()
    {
        Console.WriteLine($"Project name: {ProjectName} / Project directory: {BitPlatformProjectFolder}");

        try
        {

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

}
