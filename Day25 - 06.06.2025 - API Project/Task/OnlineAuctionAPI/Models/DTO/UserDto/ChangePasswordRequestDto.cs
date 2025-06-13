namespace OnlineAuctionAPI.Models.DTO;

using OnlineAuctionAPI.Validation;

public class ChangePasswordRequestDto
{
    public string CurrentPassword { get; set; }
    [PasswordValidation]
    public string NewPassword { get; set; }
}