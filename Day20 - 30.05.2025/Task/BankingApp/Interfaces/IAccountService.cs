using System;
using BankingApp.Models;
using BankingApp.Models.DTOs;
namespace BankingApp.Interfaces;

public interface IAccountService
{
    Task<Account> CreateAccount(Account account);
    Task<Account> GetAccount(int id);
    Task<IEnumerable<Account>> GetAllAccounts();
    Task<Account> UpdateAccount(int id, Account account);
    Task<Account> DeleteAccount(int id);
    Task<IEnumerable<AccountResponseDto>> GetAccountsByCustomerId(int customerId);

}
