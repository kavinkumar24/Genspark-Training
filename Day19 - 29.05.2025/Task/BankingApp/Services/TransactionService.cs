using BankingApp.Interfaces;
using BankingApp.Models;
using BankingApp.Models.DTOs;
using BankingApp.Repositories;


namespace BankingApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<int, Account> _accountRepository;
        private readonly IRepository<int, Transaction> _transactionRepository;


        public TransactionService(
            IRepository<int, Account> accountRepository,
            IRepository<int, Transaction> transactionRepository
            )
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

    public async Task<Transaction> CreditAmountAsync(TransferRequestDto transferRequest)
    {
    try
    {
        if (string.IsNullOrWhiteSpace(transferRequest.FromAccountNumber) || string.IsNullOrWhiteSpace(transferRequest.ToAccountNumber) || transferRequest.Amount <= 0)
        {
            throw new ArgumentException("Invalid account numbers or amount.");
        }

        var repo = _accountRepository as AccountRepository
                   ?? throw new InvalidOperationException("Repository cast failed.");

        var fromAccount = await repo.GetByAccountNumberAsync(transferRequest.FromAccountNumber);
        var toAccount = await repo.GetByAccountNumberAsync(transferRequest.ToAccountNumber);

        if (fromAccount == null || toAccount == null)
        {
            throw new Exception("One or both accounts not found.");
        }

        if (fromAccount.Balance < transferRequest.Amount)
        {
            throw new InvalidOperationException("Insufficient funds in the source account.");
        }

        fromAccount.Balance -= transferRequest.Amount;
        toAccount.Balance += transferRequest.Amount;

        await _accountRepository.Update(fromAccount.Id, fromAccount);
        await _accountRepository.Update(toAccount.Id, toAccount);

        var transaction = new Transaction
        {
            TransactionDate = DateTime.UtcNow,
            Amount = transferRequest.Amount,
            Type = TransactionType.Credit,
            FromAccountId = int.Parse(transferRequest.FromAccountNumber),
            ToAccountId = int.Parse(transferRequest.ToAccountNumber),
            Account = toAccount
        };

        return await _transactionRepository.Add(transaction);
    }
    catch (Exception ex)
    {
        throw new Exception($"Error processing credit transaction: {ex.Message}", ex);
    }
}

       

        public Task<Transaction> DebitAmountAsync(string fromAccountNumber, string toAccountNumber, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllTransfersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetTransferByIdAsync(int transactionId)
        {
            throw new NotImplementedException();
        }
    }
}