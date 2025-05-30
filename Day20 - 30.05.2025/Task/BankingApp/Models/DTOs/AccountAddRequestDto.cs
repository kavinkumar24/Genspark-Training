using System;

namespace BankingApp.Models.DTOs;

public class AccountAddRequestDto
{
    // public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public AccountType Type { get; set; }
    // public int CustomerId { get; set; }

    public ICollection<TransactionAddRequestDto>? Transactions { get; set; }
}
