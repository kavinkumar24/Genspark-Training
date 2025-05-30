using System;
using System.Numerics;

namespace BankingApp.Models.DTOs;

public class TransferRequestDto
{
    public string? FromAccountNumber { get; set; }
    public string? ToAccountNumber { get; set; }
    public decimal Amount { get; set; }
}

