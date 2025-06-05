using Notify.Models;
using Notify.Repositories;
using Notify.Interfaces;
using Notify.Models.DTO;

namespace Notify.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;

    public UserService(UserRepository userRepository, IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task AddUserAsync(UserRegisterdto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Role = userDto.Role ?? "User"
        };

        var encryptedData = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = userDto.Password
        });
        user.Password = encryptedData.EncryptedData;
        user.HashKey = encryptedData.HashKey;

        await _userRepository.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }

   
}