using Alchiweb-App1.Core.Features.Security;

namespace Alchiweb-App1.Client.Core.Infrastructure.Services;
public class CryptoService : ICryptoService
{
    private readonly IJSRuntime _js;

    public CryptoService(IJSRuntime js)
    {
        _js = js;
    }
    /* Using the minified version of this JS code:
window.cryptoHelper = {
async encrypt(plainText, keyString) {
    const enc = new TextEncoder();
    const keyMaterial = enc.encode(keyString.padEnd(32, "0")).slice(0, 32);

    const key = await crypto.subtle.importKey(
        "raw", keyMaterial, { name: "AES-GCM" }, false, ["encrypt"]
    );

    const iv = crypto.getRandomValues(new Uint8Array(12));
    const encoded = enc.encode(plainText);

    const cipherBuffer = await crypto.subtle.encrypt(
        { name: "AES-GCM", iv: iv }, key, encoded
    );

    // Combine IV + Cipher
    const combined = new Uint8Array(iv.byteLength + cipherBuffer.byteLength);
    combined.set(iv, 0);
    combined.set(new Uint8Array(cipherBuffer), iv.byteLength);

    return btoa(String.fromCharCode(...combined));
},

async decrypt(base64Data, keyString) {
    const enc = new TextEncoder();
    const keyMaterial = enc.encode(keyString.padEnd(32, "0")).slice(0, 32);

    const key = await crypto.subtle.importKey(
        "raw", keyMaterial, { name: "AES-GCM" }, false, ["decrypt"]
    );

    const combined = Uint8Array.from(atob(base64Data), c => c.charCodeAt(0));
    const iv = combined.slice(0, 12);
    const cipherText = combined.slice(12);

    const plainBuffer = await crypto.subtle.decrypt(
        { name: "AES-GCM", iv: iv }, key, cipherText
    );

    return new TextDecoder().decode(plainBuffer);
}
};
     */



    /// <summary>
    /// Encrypt the given encrypted data using AES-GCM with a predefined key.
    /// </summary>
    /// <remarks>
    /// JS from https://chandradev819.in/2025/08/13/client-side-encryption-in-blazor-webassembly-with-aes-gcm-and-jsinterop/
    /// </remarks>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> EncryptAsync(string? data, string? key = null)
        => string.IsNullOrEmpty(data) ? "" : (await _js.InvokeAsync<string>("z.x", data, string.IsNullOrEmpty(key) ? SecureDataJsonConverter.PWD_KEY : key));

    /// <summary>
    /// Decrypt the given encrypted data using AES-GCM with a predefined key.
    /// </summary>
    /// <remarks>
    /// JS from https://chandradev819.in/2025/08/13/client-side-encryption-in-blazor-webassembly-with-aes-gcm-and-jsinterop/
    /// </remarks>
    /// <param name="encrypted"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<string> DecryptAsync(string? encrypted, string? key = null)
        => string.IsNullOrEmpty(encrypted) ? "" : (await _js.InvokeAsync<string>("z.v", encrypted, string.IsNullOrEmpty(key) ? SecureDataJsonConverter.PWD_KEY : key));
}
