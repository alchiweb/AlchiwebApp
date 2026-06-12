using Microsoft.Extensions.Options;

namespace Alchiweb-App1.Client.Core.Infrastructure.Services;

public class MediaService : IMediaService
{
    
    private readonly HttpClient _httpClient;
    private readonly IToastService _toastService;
    private readonly string _apiUrl;
    private readonly ILogger<MediaService> _logger;
    private const string API_FILE_PDF = "file/pdf/";

    public MediaService(HttpClient httpClient, IOptions<ClientCoreSettings> baseUrlConfiguration, IToastService toastService, ILogger<MediaService> logger)
    {
        _httpClient = httpClient;
        _toastService = toastService;
        _apiUrl = baseUrlConfiguration.Value.ServerAddress.TrimEnd('/') + '/';
        _logger = logger;
    }

    public async Task<byte[]> GetTestPdfFile(IPdfTemplateModel? model = null)
    {
        return await GetPdfFile("Test", model);
    }

    public async Task<byte[]> GetPdfFile(string template, IPdfTemplateModel? model = null)
    {
        _logger.LogInformation("Get PDF File.");
        try
        {
            var result = await _httpClient.GetByteArrayAsync($"{_apiUrl}{API_FILE_PDF}{template}?model={(model is null ? model : JsonSerializer.Serialize(model, model.GetType(), AppJsonContext.Default))}");
            return result;
        }
        catch (Exception ex)
        {
            _toastService.Error($"Error: {ex.Message}");
            return Array.Empty<byte>();
        }
    }
}
