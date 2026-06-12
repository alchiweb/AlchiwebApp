namespace Alchiweb-App1.Core.Features.Security;

public interface ICryptoService
{
    public Task<string> EncryptAsync(string? data, string? key = null);
    public Task<string> DecryptAsync(string? encryptedData, string? key = null);
}
