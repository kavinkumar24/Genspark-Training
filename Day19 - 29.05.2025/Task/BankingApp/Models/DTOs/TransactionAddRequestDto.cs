using System;

namespace BankingApp.Models.DTOs;

public class TransactionAddRequestDto
{
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public int AccountId { get; set; }

}
