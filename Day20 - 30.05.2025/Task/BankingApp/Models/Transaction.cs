namespace BankingApp.Models
{
public class Transaction
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }

    public string FromAccountNumber { get; set; } = null!;
    public Account FromAccount { get; set; } = null!;

    public string ToAccountNumber { get; set; } = null!;
    public Account ToAccount { get; set; } = null!;
}


}