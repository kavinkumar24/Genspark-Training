namespace BankingApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public AccountType Type { get; set; }
        public string Status { get; set; } = "Active"; 
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}