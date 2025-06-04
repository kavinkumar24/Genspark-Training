
using System.IdentityModel.Tokens.Jwt;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth;


namespace FirstAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _config;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptionService encryptionService,
                                    IRepository<string, User> userRepository,
                                    ILogger<AuthenticationService> logger, IConfiguration config)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _logger = logger;
            _config = config;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await _userRepository.Get(user.Username);
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
            for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
            {
                if (encryptedData.EncryptedData[i] != dbUser.Password[i])
                {
                    _logger.LogError("Invalid login attempt");
                    throw new Exception("Invalid password");
                }
            }
            var token = await _tokenService.GenerateToken(dbUser);
            return new UserLoginResponse
            {
                Username = user.Username,
                Token = token,
            };
        }

        public async Task<UserLoginResponse> LoginWithGoogleAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _config["Google:ClientId"] }
                });
                if (payload == null)
                {
                    _logger.LogError("Invalid Google token");
                    throw new UnauthorizedAccessException("Invalid Google token");
                }
                var email = payload.Email;

                var user = await _userRepository.Get(email);
                if (user == null)
                    throw new UnauthorizedAccessException("User not found in system");

                var jwtToken = await _tokenService.GenerateToken(user);

                return new UserLoginResponse
                {
                    Username = email,
                    Token = jwtToken
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google login");
                throw new UnauthorizedAccessException("Google login failed", ex);
            }
        }

        // public async Task<UserLoginResponse> LoginWithGoogleAsync(string idToken)
        // {
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var validationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidIssuer = "https://accounts.google.com",
        //         ValidateAudience = true,
        //         ValidAudience = _config["Google:ClientId"], 
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true
        //     };

        //     var principal = tokenHandler.ValidateToken(idToken, validationParameters, out _);

        //     var email = principal.FindFirst("email")?.Value;
        //     if (string.IsNullOrEmpty(email))
        //         throw new UnauthorizedAccessException("Invalid Google token");

        //     var user = await _userRepository.Get(email);
        //     if (user == null)
        //         throw new UnauthorizedAccessException("User not found in system");

        //     var jwtToken = await _tokenService.GenerateToken(user);

        //     return new UserLoginResponse
        //     {
        //         Username = user.Username,
        //         Token = jwtToken
        //     };
        // }

        
    }
}