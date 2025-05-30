using System;
using BankingApp.Models;
using BankingApp.Models.DTOs;

namespace BankingApp.Interfaces;
public interface ITransactionService
{
    Task<Transaction> TransferAmountAsync(TransferRequestDto transferRequestDto);
    Task<Transaction> DebitAmountAsync(TransferRequestDto debitRequestDto);
    Task<IEnumerable<Transaction>> GetAllTransfersAsync();
    Task<Transaction> GetTransferByIdAsync(int transactionId);
}
