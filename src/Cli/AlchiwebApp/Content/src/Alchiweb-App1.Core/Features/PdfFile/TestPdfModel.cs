using AlchiwebApp.Core.Interfaces;
using System.Text.Json.Serialization;

namespace Alchiweb-App1.Core.Features.PdfFile;

public class TestPdfModel : IPdfTemplateModel
{
    #region Properties
    public string Message { get; set; }
    #endregion

    #region Constructors
    [JsonConstructor]
    public TestPdfModel(string message)
    {
        Message = message;
    }
    #endregion
}
