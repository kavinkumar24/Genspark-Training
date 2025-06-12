namespace OnlineAuctionAPI.Validation;

using OnlineAuctionAPI.Helpers;
using System.ComponentModel.DataAnnotations;

 public class PasswordValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string password = value as string ?? "";
            InputValidator.ValidateString(password, nameof(password));
            if (!(password.Length > 3) || password.Contains(' '))
                return false;
            string allowedSpecials = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~\\";
            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c) && !allowedSpecials.Contains(c))
                    return false;
            }
            return true;
        }
    }