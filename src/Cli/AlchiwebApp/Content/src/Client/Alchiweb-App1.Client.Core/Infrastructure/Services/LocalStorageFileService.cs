using Blazored.LocalStorage;
using Alchiweb-App1.Client.Core;
using System.Net.Http;
using System.Net.Http.Json;

namespace Alchiweb-App1.Client.Core.Infrastructure.Services;

public class LocalStorageFileService(HttpClient _httpClient, ILocalStorageService _localStorage) : ILocalFileService
{
    public async Task<TValue?> GetFromJsonAsync<TValue>(string filename) where TValue : class
    {
        try
        {
            return await _httpClient.GetFromJsonAsync(filename, typeof(TValue), ClientJsonContext.Default) as TValue;
        }
        catch (InvalidOperationException) { return null; }
    }

    public async Task<string?> GetItemAsync(string key)
    {
        try
        {
            return await _localStorage.GetItemAsync<string>(key);
        }
        catch (InvalidOperationException) { return null; }
    }

        
    public async Task RemoveItemAsync(string key)
    {
        try
        {
            await _localStorage.RemoveItemAsync(key);
        }
        catch (InvalidOperationException) { }
    }


    public async Task SetItemAsync(string key, string data)
    {
        try
        {
            await _localStorage.SetItemAsync(key, data);
        }
        catch (InvalidOperationException) { }
    }
}
