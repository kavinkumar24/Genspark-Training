using System;

namespace BankingApp.Misc;

using BankingApp.Contexts;
using BankingApp.Interfaces;
using BankingApp.Models;
using BankingApp.Models.DTOs;
using BankingApp.Repositories;

public class AccountMapper
{
    public Account? MapAccountAddRequest(AccountAddRequestDto request)
    {
        if (request == null)
        {
            return null;
        }

        return new Account
        {
            AccountNumber = UniqueAccountNumber(),
            Balance = request.Balance,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public string UniqueAccountNumber()
    {
        var prefix = "1202";
        Random random = new Random();
        int randomNumber = random.Next(0, 10000);
        string suffix = randomNumber.ToString("D2");
        return $"{prefix}{suffix}";
    }

}
