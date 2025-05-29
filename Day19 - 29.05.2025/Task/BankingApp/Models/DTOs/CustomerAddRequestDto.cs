namespace BankingApp.Models.DTOs
{
    public class CustomerAddRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;

         public ICollection<AccountAddRequestDto>? Accounts { get; set; }
    }
}