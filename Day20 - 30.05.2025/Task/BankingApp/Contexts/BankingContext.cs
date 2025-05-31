using Microsoft.EntityFrameworkCore;
using BankingApp.Models;
using BankingApp.Models.DTOs;
using Banking.Models;
namespace BankingApp.Contexts
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<AccountResponseDto> AccountResponseDtos { get; set; } = null!;
        public DbSet<ChatResponse> ChatResponses { get; set; }
        public DbSet<ChatTrainingData> ChatTrainingData { get; set; } = null!;
        public DbSet<ChatPredictionLog> ChatPredictionLogs { get; set; }

        

        public async Task<List<AccountResponseDto>> GetAccountsByCustomerId(int customerId)
        {
            return await this.Set<AccountResponseDto>()
                        .FromSqlInterpolated($"select * from GetAccountsByCustomerId({customerId})")
                        .ToListAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().HasIndex(c => c.PhoneNumber)
                .IsUnique();
            

            modelBuilder.Entity<Customer>().HasMany(c => c.Accounts)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>().HasKey(a => a.Id);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.SentTransactions)
                .HasForeignKey(t => t.FromAccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.ReceivedTransactions)
                .HasForeignKey(t => t.ToAccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);

            
            

        }
    }
}