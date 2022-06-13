using System.Threading.Tasks;

namespace BWJ.Web.Core.Security
{
    public interface IEncryptionService
    {
        Task<byte[]> DecryptData(byte[] data, string IV = null);
        Task<string> DecryptText(string text, string IV = null);
        Task<byte[]> EncryptData(byte[] data, string IV = null);
        Task<string> EncryptText(string text, string IV = null);
    }
}