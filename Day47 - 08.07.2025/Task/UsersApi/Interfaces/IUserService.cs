using UserApi.Models;

namespace UserApi.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
}
