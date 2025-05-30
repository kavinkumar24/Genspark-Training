
using BankingApp.Models.DTOs;
using BankingApp.Contexts;
using BankingApp.Interfaces;
using BankingApp.Models;
namespace BankingApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly IOtherFunctionalitiesInterface _otherFunctionalities;
        
        private readonly IRepository<int, Account> _accountRepository;
        public AccountService(IOtherFunctionalitiesInterface otherFunctionalities, IRepository<int, Account> accountRepository)
        {
            _accountRepository = accountRepository;
            _otherFunctionalities = otherFunctionalities;
        }

        public Task<Account> CreateAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> DeleteAccount(int id)
        {
            var account = await _accountRepository.Get(id);
            if (account == null)
            {
                throw new Exception($"Account with ID {id} not found");
            }
            return await _accountRepository.Delete(id);
        }

        public Task<Account> GetAccount(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAccountsByCustomerId(int customerId)
        {

            var accounts = await _otherFunctionalities.GetAccountsByCustomerId(customerId);
            if (accounts == null || !accounts.Any())
            {
                throw new Exception($"No accounts found for customer with ID {customerId}");
            }

            return accounts;

        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            var acccounts = await _accountRepository.GetAll();
            if (acccounts == null || !acccounts.Any())
            {
                throw new Exception("No accounts found");
            }
            return acccounts;
        }

        public Task<Account> UpdateAccount(int id, Account account)
        {
            var existingAccount = _accountRepository.Get(id);
            if (existingAccount == null)
            {
                throw new Exception($"Account with ID {id} not found");
            }
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }
            return _accountRepository.Update(id, account);
        }
    }
}