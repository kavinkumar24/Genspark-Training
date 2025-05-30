
namespace BankingApp.Models.DTOs
{
    public class CreditRequestDto
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}