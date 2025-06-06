using Notify.Validation;
namespace Notify.Models.DTO;
using System.ComponentModel.DataAnnotations;

public class UserRegisterdto
{
    [NameValidation(ErrorMessage = "Invalid name specified. Please use only letters.")]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;
    
    [RoleValidation(ErrorMessage = "Invalid role specified. please specify either 'User' or 'HR'.")]
    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; } = string.Empty;

    [PasswordValidation]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}