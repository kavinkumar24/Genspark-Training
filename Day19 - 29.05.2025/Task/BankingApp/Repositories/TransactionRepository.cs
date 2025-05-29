using System;
using BankingApp.Contexts;
using BankingApp.Models;
namespace BankingApp.Repositories;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : Repository<int, Transaction>
{
    public TransactionRepository(BankingContext context) : base(context)
    {
    }

    public override async Task<Transaction> Get(int key)
    {
        if (key <= 0)
        {
            throw new ArgumentException("Transaction ID must be greater than zero.");
        }
        var transaction = await _bankingContext.Transactions
            .SingleOrDefaultAsync(t => t.Id == key);
        if (transaction == null)
        {
            throw new Exception("Transaction not found");
        }
        return transaction;
    }
    public override async Task<IEnumerable<Transaction>> GetAll()
    {
        try
        {
            return await _bankingContext.Transactions.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all transactions: {ex.Message}");
        }
    }
   
}