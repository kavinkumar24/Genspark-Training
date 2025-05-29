using System;
using BankingApp.Contexts;
using BankingApp.Interfaces;
using BankingApp.Models;
using BankingApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Repositories;

public class AccountRepository : Repository<int, Account>
{


    public AccountRepository(BankingContext context) : base(context)
    {
    }

    public override async Task<Account> Get(int key)
    {
        if (key <= 0)
        {
            throw new ArgumentException("Account ID must be greater than zero.");
        }
        var account = await _bankingContext.Accounts.SingleOrDefaultAsync(c => c.Id == key);
        if (account == null)
        {
            throw new Exception("Customer not found");
        }
        return account;
    }

    public override async Task<IEnumerable<Account>> GetAll()
    {
        try
        {
            return await _bankingContext.Accounts.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all accounts: {ex.Message}");
        }
    }
    
  public async Task<Account> GetByAccountNumberAsync(string accountNumber)
{
    return await _bankingContext.Accounts
                         .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
}

}
