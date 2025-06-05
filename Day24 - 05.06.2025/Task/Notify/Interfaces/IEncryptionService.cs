using Notify.Models;
namespace Notify.Interfaces;

public interface IEncryptionService
{
    Task<EncryptModel> EncryptData(EncryptModel data);
}