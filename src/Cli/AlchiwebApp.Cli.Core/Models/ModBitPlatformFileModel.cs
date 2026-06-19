namespace AlchiwebApp.Cli.Core.Models;

public class ModBitPlatformFileModel
{
    public string Filename { get; set; }
    public string SearchText { get; set; }
    public string ReplaceText { get; set; }
    public ActionEnum Action { get; set; }

}
