using AlchiwebApp.Core.Interfaces;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alchiweb-App1.Core.Features.Security;

public class SecureDataJsonConverter : JsonConverter<string?>
{
    public static readonly string PWD_KEY = "3[Vu0>T^4;f39AD)AHHZ8jrVsb&%N1p;5k8xJe8*ufGp{G.b]1V>8KtJ,[v";

    /// <summary>
    /// NOT for WASM browser (without decryption here -> must call the decrypt function in JS)
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        var bytesStr = Convert.FromBase64String(str);
        var hash = bytesStr.Take(32).ToArray();
        var data = bytesStr.Skip(32).ToArray();

        SHA256 sha = SHA256.Create();
        
        if (data == null || hash == null || !string.Equals(BitConverter.ToString(hash), BitConverter.ToString(sha.ComputeHash(data))))
        {
             return null;
        }
        str = Encoding.UTF8.GetString(data);

        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        Guid id = Guid.Empty;

        string password = str;

        if (str.Length > 32)
        {
            if (Guid.TryParse(str.Substring(0, 32), out id))
            {
                password = str.Substring(32, str.Length - 32);
            }
        }
        if (string.IsNullOrEmpty(password))
        {
            return null;
        }

        password = Decrypt(password, PWD_KEY);
        if (string.IsNullOrEmpty(password))
        {
            return null;
        }
        return password;
    }

    /// <summary>
    /// For WASM browser (without encryption here -> must call the encrypt function in JS)
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (string.IsNullOrEmpty(value))
        {
            writer.WriteNullValue();
            return;
        }
        // value must be encrypted before (if WASM browser, call the encrypt function in JS)
        var str = value ?? "";

        var bytesStr = Encoding.UTF8.GetBytes(str);
        SHA256 sha = SHA256.Create();
        sha.ComputeHash(bytesStr).Concat(bytesStr);
        str = Convert.ToBase64String(sha.ComputeHash(bytesStr).Concat(bytesStr).ToArray());

        if (string.IsNullOrEmpty(str))
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(str);
    }

    public override bool HandleNull => true;
    /*
    //TODO: optimization required (and to be similar to decrypt)
    public static string Encrypt(string data, string key)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Data");
            // convert from base64 to raw bytes spans

            var dataBytes = Encoding.UTF8.GetBytes(data).AsSpan();
            var keyBytes = Encoding.UTF8.GetBytes(key).Take(32).ToArray();

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
            var ivi = Encoding.UTF8.GetBytes(PWD_KEY).Take(nonceSize).ToArray().AsSpan();
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
    */
    /// <summary>
    /// Decrypt the given encrypted data using AES-GCM with a predefined keyBytes.
    /// NOT for WASM browser!
    /// </summary>
    /// <remarks>
    /// Code from https://pilabor.com/series/dotnet/js-gcm-encrypt-dotnet-decrypt/
    /// </remarks>
    /// <param name="encryptedData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Decrypt(string encryptedData, string key)
    {
        try
        {
            // convert from base64 to raw bytes spans
            var encryptedDataBytes = Convert.FromBase64String(encryptedData).AsSpan();
            var keyBytes = Encoding.UTF8.GetBytes(PWD_KEY).Take(32).ToArray().AsSpan();

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
