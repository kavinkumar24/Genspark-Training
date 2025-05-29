using System;
using BankingApp.Models;

namespace BankingApp.Interfaces;
public interface ITransactionService
{
    Task<Transaction> TransferAmountAsync(int fromAccountId, int toAccountId, decimal amount);
    Task<IEnumerable<Transaction>> GetAllTransfersAsync();
    Task<Transaction> GetTransferByIdAsync(int transactionId);
}

