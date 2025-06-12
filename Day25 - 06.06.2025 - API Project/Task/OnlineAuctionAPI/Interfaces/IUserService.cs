
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(UserRegisterRequestDto userDto);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> UpdateUserInfoAsync(Guid id, UserUpdateRequestDto updateuserdto);
    Task<User> DeleteUserAsync(Guid id);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

}