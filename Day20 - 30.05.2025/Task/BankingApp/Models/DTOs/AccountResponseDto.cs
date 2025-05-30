namespace BankingApp.Models.DTOs
{
    public class AccountResponseDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
    }
}

