namespace Notify.Validation;
using System.ComponentModel.DataAnnotations;


public class RoleValidation : ValidationAttribute
{
    private readonly string[] _validRoles = { "User", "HR"};

    public override bool IsValid(object? value)
    {
        if (value is not string role || string.IsNullOrEmpty(role))
            return false;

        return Array.Exists(_validRoles, r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be one of the following roles: {string.Join(", ", _validRoles)}.";
    }
}