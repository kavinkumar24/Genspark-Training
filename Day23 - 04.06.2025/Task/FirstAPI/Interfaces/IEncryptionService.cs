using FirstAPI.Models;

namespace FirstAPI.Interfaces
{
    public interface IEncryptionService
    {
        Task<EncryptModel> EncryptData(EncryptModel data);
    }
}