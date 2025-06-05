using Notify.Models;
using Notify.Models.DTO;
namespace Notify.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task AddUserAsync(UserRegisterdto user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}