using BankingApp.Contexts;
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

        private readonly BankingContext _bankingContext;
        public TransactionService(
            IRepository<int, Account> accountRepository,
            IRepository<int, Transaction> transactionRepository,
            BankingContext bankingContext
            )
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _bankingContext = bankingContext;
        }

    public async Task<Transaction> TransferAmountAsync(TransferRequestDto transferRequest)
    {
        using var transaction = await _bankingContext.Database.BeginTransactionAsync();
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

                var transactionEntry = new Transaction
                {
                    TransactionDate = DateTime.UtcNow,
                    Amount = transferRequest.Amount,
                    Type = TransactionType.Credit,
                    FromAccountNumber = transferRequest.FromAccountNumber,
                    ToAccountNumber = transferRequest.ToAccountNumber,
                    FromAccount = fromAccount,
                    ToAccount = toAccount,
                };

                var result = await _transactionRepository.Add(transactionEntry);
                await transaction.CommitAsync();
                return result;
            
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error processing credit transaction: {ex.Message}", ex);
            }
}

       

        public async Task<Transaction> DebitAmountAsync(TransferRequestDto transferRequest)
        {
            using var transaction = await _bankingContext.Database.BeginTransactionAsync();
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

                var transactionEntry = new Transaction
                {
                    TransactionDate = DateTime.UtcNow,
                    Amount = transferRequest.Amount,
                    Type = TransactionType.Debit,
                    FromAccountNumber = transferRequest.FromAccountNumber,
                    ToAccountNumber = transferRequest.ToAccountNumber,
                    FromAccount = fromAccount,
                    ToAccount = toAccount,
                };

                var result = await _transactionRepository.Add(transactionEntry);
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing debit transaction: {ex.Message}", ex);
            }
        }
        

        public Task<IEnumerable<Transaction>> GetAllTransfersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetTransferByIdAsync(int transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> CreditAmountAsync(CreditRequestDto creditRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> DebitAmountAsync(DebitRequestDto debitRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}