using AlchiwebApp.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace AlchiwebApp.Server.Core.Interfaces;

public interface IFileService
{
    Task GetPdfStreamAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TPdfTemplate>(Stream newMemoryStream, IPdfTemplateModel? model = null) where TPdfTemplate : IPdfTemplate<IPdfTemplateModel>;
    Task GetPdfStreamAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type pdfTemplateType, Stream newMemoryStream, IPdfTemplateModel? model = null);
}
