

using BankingApp.Contexts;
using BankingApp.Interfaces;
using BankingApp.Models.DTOs;

namespace BankingApp.Misc
{
    public  class OtherFunctionalities : IOtherFunctionalitiesInterface
    {
        private readonly BankingContext _bankingContext;

        public OtherFunctionalities(BankingContext bankingContext)
        {
            _bankingContext = bankingContext;
        }
        public async Task<IEnumerable<AccountResponseDto>> GetAccountsByCustomerId(int customerId)
        {
            var result = await _bankingContext.GetAccountsByCustomerId(customerId);
            return result;
        }
    }
}