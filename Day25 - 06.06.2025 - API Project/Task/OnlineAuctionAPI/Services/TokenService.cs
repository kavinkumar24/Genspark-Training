using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
namespace OnlineAuctionAPI.Service;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OnlineAuctionAPI.Models.DTO;
using System.Security.Cryptography;
using OnlineAuctionAPI.Contexts;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _securityKey;
    private readonly AuctionContext _auctionContext;
    private readonly int _accessTokenExpiryMinutes;
    private readonly int _refreshTokenExpiryDays;
    private readonly IUserRepository _userRepository;
    public TokenService(AuctionContext auctionContext, IConfiguration configuration, IUserRepository userRepository)
    {
        _auctionContext = auctionContext;
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Keys:JwtTokenKey"]));
        _accessTokenExpiryMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpiryMinutes"]);
        _refreshTokenExpiryDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"]);
        _userRepository = userRepository;


    }
    public async Task<TokenDto> GenerateTokensAsync(User user)
    {
        List<Claim> claims = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim("UserId", user.Id.ToString()),
            };

        var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = GenerateRefreshToken();
        var newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays),
            IsRevoked = false,
            UserId = user.Id
        };
        _auctionContext.RefreshTokens.Add(newRefreshToken);
        await _auctionContext.SaveChangesAsync();
        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string GenerateRefreshToken()
    {

        var randomBytes = new byte[64];
        using var randomNumGen = RandomNumberGenerator.Create();
        randomNumGen.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);

    }
    public async Task<UserLoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var storedRefreshToken = await _userRepository.GetRefreshTokenAsync(refreshToken);
        if (storedRefreshToken == null || storedRefreshToken.ExpiresAt < DateTime.UtcNow || storedRefreshToken.IsRevoked)
        {
            throw new UnauthorizedAccessException("Invalid, expired, or revoked refresh token");
        }

        var user = await _userRepository.Get(storedRefreshToken.UserId);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found for refresh token");
        }
       

        storedRefreshToken.IsRevoked = true;
        await _auctionContext.SaveChangesAsync();

        var newTokens = await GenerateTokensAsync(user);

        return new UserLoginResponseDto
        {
            UserName = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken
        };
    }

}
        