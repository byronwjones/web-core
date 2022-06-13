using System.Threading.Tasks;

namespace BWJ.Web.Core.Security
{
    public interface IEncryptionServiceSettingsProvider
    {
        Task<byte[]> GetInitializationVector(string vector);
        Task<byte[]> GetKey();
    }
}