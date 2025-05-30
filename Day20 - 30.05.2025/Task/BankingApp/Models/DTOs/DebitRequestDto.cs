namespace BankingApp.Models.DTOs
{
    public class DebitRequestDto
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}