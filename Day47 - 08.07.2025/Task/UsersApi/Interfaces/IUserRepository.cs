using UserApi.Models;

namespace UserApi.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
}
