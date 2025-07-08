using UserApi.Interfaces;
using UserApi.Models;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return await _userRepository.AddAsync(user);
    }
}
