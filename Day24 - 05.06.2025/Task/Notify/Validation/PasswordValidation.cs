namespace Notify.Validation;
using System.ComponentModel.DataAnnotations;

 public class PasswordValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string password = value as string ?? "";
            if (string.IsNullOrEmpty(password))
                return false;
            if (!(password.Length > 3) || !password.Contains(' '))
                return false;
            foreach (char c in password)
            {
                if (!char.IsLetter(c) || !char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }