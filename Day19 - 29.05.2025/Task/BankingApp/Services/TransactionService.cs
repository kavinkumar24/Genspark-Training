using BankingApp.Interfaces;
using BankingApp.Models;


namespace BankingApp.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<int, Account> _accountRepository;
        private readonly IRepository<int, Transaction> _transactionRepository;


        public TransactionService(
            IRepository<int, Account> accountRepository,
            IRepository<int, Transaction> transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> TransferAmountAsync(int fromAccountId, int toAccountId, decimal amount)
        {
            if (fromAccountId <= 0 || toAccountId <= 0)
                throw new ArgumentException("Account IDs must be positive.");


            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be positive.");


            if (fromAccountId == toAccountId)
                throw new ArgumentException("Cannot transfer to the same account.");


            var fromAccount = await _accountRepository.Get(fromAccountId);
            var toAccount = await _accountRepository.Get(toAccountId);


            if (fromAccount == null)
                throw new Exception("From account not found.");
            if (toAccount == null)
                throw new Exception("To account not found.");


            if (fromAccount.Balance < amount)
                throw new Exception("Insufficient balance.");


            fromAccount.Balance -= amount;
            toAccount.Balance += amount;


            await _accountRepository.Update(fromAccount.Id, fromAccount);
            await _accountRepository.Update(toAccount.Id, toAccount);


            var transaction = new Transaction
            {
                AccountId = fromAccountId,
                TransactionDate = DateTime.UtcNow,
                Amount = amount,
                Type = TransactionType.Debit
            };


            await _transactionRepository.Add(transaction);


            var creditTransaction = new Transaction
            {
                AccountId = toAccountId,
                TransactionDate = DateTime.UtcNow,
                Amount = amount,
                Type = TransactionType.Credit
            };


            await _transactionRepository.Add(creditTransaction);


            return transaction;
        }

        Task<IEnumerable<Transaction>> ITransactionService.GetAllTransfersAsync()
        {
            try
            {
                return _transactionRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all transactions: {ex.Message}", ex);
            }
        }

        Task<Transaction> ITransactionService.GetTransferByIdAsync(int transactionId)
        {
            if (transactionId <= 0)
            {
                throw new ArgumentException("Transaction ID must be greater than zero.");
            }

            try
            {
                return _transactionRepository.Get(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving transaction by ID: {ex.Message}", ex);
            }
        }
    }
}