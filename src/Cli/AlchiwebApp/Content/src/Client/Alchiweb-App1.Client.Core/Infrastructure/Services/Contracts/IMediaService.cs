namespace Alchiweb-App1.Client.Core.Infrastructure.Services.Contracts;

public interface IMediaService
{
    Task<byte[]> GetTestPdfFile(IPdfTemplateModel? model = null);
    Task<byte[]> GetPdfFile(string template, IPdfTemplateModel? model = null);
}
