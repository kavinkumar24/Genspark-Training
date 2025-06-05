using Notify.Interfaces;
using Notify.Models.DTO;
using Notify.Repositories;
using Notify.Models;

namespace Notify.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        UserRepository userRepository,
        IEncryptionService encryptionService,
        ITokenService tokenService,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest user)
    {
   
        var dbUser = (await _userRepository.GetAllAsync())
            .FirstOrDefault(u => u.Email == user.Email);

        if (dbUser == null)
        {
            _logger.LogCritical("User not found");
            throw new Exception("No such user");
        }

        var encryptedData = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = user.Password,
            HashKey = dbUser.HashKey
        });

        if (encryptedData.EncryptedData == null || dbUser.Password == null || !encryptedData.EncryptedData.SequenceEqual(dbUser.Password))
        {
            _logger.LogError("Invalid login attempt");
            throw new Exception("Invalid password");
        }

        var token = await _tokenService.GenerateToken(dbUser);

        return new UserLoginResponse
        {
            Email = user.Email,
            Token = token,
        };
    }
}