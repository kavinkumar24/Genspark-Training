
using BankingApp.Models.DTOs;
namespace BankingApp.Interfaces
{
    public interface IOtherFunctionalitiesInterface
    {

        Task<IEnumerable<AccountResponseDto>> GetAccountsByCustomerId(int customerId);
    }
}