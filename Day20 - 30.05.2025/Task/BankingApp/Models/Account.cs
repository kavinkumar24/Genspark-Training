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
    public Customer Customer { get; set; } = null!;

    public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
}

}