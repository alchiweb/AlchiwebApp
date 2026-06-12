namespace Alchiweb-App1.Client.Core.Infrastructure.Services.Contracts;

public interface ILocalFileService
{
    Task<TValue?> GetFromJsonAsync<TValue>(string filename) where TValue : class;
    Task<string?> GetItemAsync(string key);
    Task SetItemAsync(string key, string data);
    Task RemoveItemAsync(string key);
}
