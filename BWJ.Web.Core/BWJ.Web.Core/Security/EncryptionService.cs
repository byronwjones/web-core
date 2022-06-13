using BWJ.Web.Core.Attributes;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BWJ.Web.Core.Security
{
    [ApplicationService(Sentinels.ApplicationServiceLifetime.Singleton)]
    public class EncryptionService : IEncryptionService
    {
        private readonly IEncryptionServiceSettingsProvider _settingsProvider;

        public EncryptionService(
            IEncryptionServiceSettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task<string> EncryptText(string text, string IV = default)
        {
            byte[] result = Encoding.UTF8.GetBytes(text);
            result = await EncryptData(result, IV);

            return Convert.ToBase64String(result);
        }

        public async Task<string> DecryptText(string text, string IV = default)
        {
            byte[] binaryData = Convert.FromBase64String(text);
            binaryData = await DecryptData(binaryData, IV);

            return Encoding.UTF8.GetString(binaryData);
        }

        public async Task<byte[]> EncryptData(byte[] data, string IV = default)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = await _settingsProvider.GetKey();
                aes.IV = await _settingsProvider.GetInitializationVector(IV);
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                using (var encDataStream = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(encDataStream, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                    }

                    return encDataStream.ToArray();
                }
            }
        }

        public async Task<byte[]> DecryptData(byte[] data, string IV = default)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = await _settingsProvider.GetKey();
                aes.IV = await _settingsProvider.GetInitializationVector(IV);
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var decDataStream = new MemoryStream())
                {
                    using (var csDecrypt = new CryptoStream(decDataStream, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(data, 0, data.Length);
                    }

                    return decDataStream.ToArray();
                }
            }
        }

    }
}
