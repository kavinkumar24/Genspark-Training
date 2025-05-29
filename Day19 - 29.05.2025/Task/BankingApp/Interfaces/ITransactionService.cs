using System;
using BankingApp.Models;
using BankingApp.Models.DTOs;

namespace BankingApp.Interfaces;
public interface ITransactionService
{
    Task<Transaction> CreditAmountAsync(TransferRequestDto transferRequestDto);
    Task<Transaction> DebitAmountAsync(string fromAccountNumber, string toAccountNumber, decimal amount);
    Task<IEnumerable<Transaction>> GetAllTransfersAsync();
    Task<Transaction> GetTransferByIdAsync(int transactionId);
}
