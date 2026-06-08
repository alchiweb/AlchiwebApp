namespace AlchiwebApp.Cli.Core.Services;

// From https://github.com/valginer0/WinFindGrep
public class SearchResult
{
    public string FilePath { get; set; } = "";
    public int LineNumber { get; set; }
    public string LineContent { get; set; } = "";
    public DateTime LastModified { get; set; }
}
