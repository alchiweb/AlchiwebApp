using System.Security.Cryptography;
using System.Text;
using Alchiweb-App1.Core.Features.Security;

namespace Alchiweb-App1.Server.Core.Features.Security;

public class CryptoService : ICryptoService
{
    public CryptoService()
    {
    }

    /// <summary>
    /// Encrypt the given encrypted data using AES-GCM with a predefined key.
    /// </summary>
    /// <remarks>
    /// JS from https://chandradev819.in/2025/08/13/client-side-encryption-in-blazor-webassembly-with-aes-gcm-and-jsinterop/
    /// </remarks>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<string> EncryptAsync(string? data, string? key = null)
        => Task.FromResult(string.IsNullOrEmpty(data) ? "" : Encrypt(data,key));

    //TODO: optimization required (and to be similar to decrypt)
    public string Encrypt(string? data, string? key = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Data");
            // convert from base64 to raw bytes spans

            var dataBytes = Encoding.UTF8.GetBytes(data).AsSpan();
            var keyBytes = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(key) ? SecureDataJsonConverter.PWD_KEY : key).Take(32).ToArray();

            // Get parameter sizes
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;  // 12 bytes
            int tagSize = AesGcm.TagByteSizes.MaxSize; // 16 bytes
            int cipherSize = dataBytes.Length;

            // We write everything into one big array for easier encoding
            int encryptedDataLength = nonceSize + cipherSize + tagSize;
            Span<byte> encryptedData = encryptedDataLength < 1024
                                     ? stackalloc byte[encryptedDataLength]
                                     : new byte[encryptedDataLength].AsSpan();
            var cipherText = new byte[dataBytes.Length];
            // Copy parameters
            var ivi = Encoding.UTF8.GetBytes(SecureDataJsonConverter.PWD_KEY).Take(nonceSize).ToArray().AsSpan();
            // Generate secure nonce
            RandomNumberGenerator.Fill(ivi);
            var tag = new byte[tagSize];
            RandomNumberGenerator.Fill(tag);

            // Encrypt
            using var aes = new AesGcm(keyBytes, tagSize);
            aes.Encrypt(ivi, dataBytes, cipherText, tag);

            // Encode for transmission
            return Convert.ToBase64String(ivi.ToArray().Concat(cipherText.Concat(tag)).ToArray());
        }
        catch (Exception)
        {
            return "";
        }
    }

    /// <summary>
    /// Decrypt the given encrypted data using AES-GCM with a predefined key.
    /// </summary>
    /// <remarks>
    /// JS from https://chandradev819.in/2025/08/13/client-side-encryption-in-blazor-webassembly-with-aes-gcm-and-jsinterop/
    /// </remarks>
    /// <param name="encrypted"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<string> DecryptAsync(string? encrypted, string? key = null)
        => Task.FromResult(string.IsNullOrEmpty(encrypted) ? "" : Decrypt(encrypted, key));

    /// <summary>
    /// Decrypt the given encrypted data using AES-GCM with a predefined keyBytes.
    /// </summary>
    /// <remarks>
    /// Code from https://pilabor.com/series/dotnet/js-gcm-encrypt-dotnet-decrypt/
    /// </remarks>
    /// <param name="encryptedData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Decrypt(string? encryptedData, string? key = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(encryptedData))
                throw new ArgumentException("Data");
            // convert from base64 to raw bytes spans
            var encryptedDataBytes = Convert.FromBase64String(encryptedData).AsSpan();
            var keyBytes = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(key) ? SecureDataJsonConverter.PWD_KEY : key).Take(32).ToArray().AsSpan();

            var tagSizeBytes = 16; // 128 bit encryption / 8 bit = 16 bytes
            var ivSizeBytes = 12; // 12 bytes iv

            // ciphertext size is whole data - iv - tag
            var cipherSize = encryptedDataBytes.Length - tagSizeBytes - ivSizeBytes;

            // extract iv (nonce) 12 bytes prefix
            var iv = encryptedDataBytes.Slice(0, ivSizeBytes);

            // followed by the real ciphertext
            var cipherBytes = encryptedDataBytes.Slice(ivSizeBytes, cipherSize);

            // followed by the tag (trailer)
            var tagStart = ivSizeBytes + cipherSize;
            var tag = encryptedDataBytes.Slice(tagStart);

            // now that we have all the parts, the decryption
            Span<byte> plainBytes = cipherSize < 1024
                ? stackalloc byte[cipherSize]
                : new byte[cipherSize];
            using var aes = new AesGcm(keyBytes, tagSizeBytes);
            aes.Decrypt(iv, cipherBytes, tag, plainBytes);
            return Encoding.UTF8.GetString(plainBytes);
        }
        catch (Exception)
        {
            return "";
        }
    }


}
