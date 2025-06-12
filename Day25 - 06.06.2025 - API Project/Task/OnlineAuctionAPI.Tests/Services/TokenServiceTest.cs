using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Service;
using OnlineAuctionAPI.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

[TestFixture]
public class TokenServiceTests
{
    private AuctionContext _context;
    private TokenService _tokenService;
    private Mock<IUserRepository> _mockUserRepository;
    private IConfiguration _configuration;
    private User _testUser;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AuctionContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AuctionContext(options);

        var inMemorySettings = new Dictionary<string, string>
        {
            {"Keys:JwtTokenKey", "this_is_a_secure_jwt_token_key_123456789"},
            {"JwtSettings:AccessTokenExpiryMinutes", "15"},
            {"JwtSettings:RefreshTokenExpiryDays", "7"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _mockUserRepository = new Mock<IUserRepository>();
        _tokenService = new TokenService(_context, _configuration, _mockUserRepository.Object);

        _testUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Role = UserRole.Bidder,
            Username = "TestUser"
        };
    }

    [Test]
    public async Task GenerateTokensAsync_ValidUser_ReturnsTokenDto()
    {
        // Act
        var result = await _tokenService.GenerateTokensAsync(_testUser);

        // Assert
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotNull(result.RefreshToken);
        Assert.IsTrue(_context.RefreshTokens.AnyAsync().Result);
    }

    [Test]
    public async Task RefreshTokenAsync_ValidToken_ReturnsNewTokens()
    {
        // Arrange
        var tokenResult = await _tokenService.GenerateTokensAsync(_testUser);
        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenResult.RefreshToken);

        _mockUserRepository.Setup(x => x.GetRefreshTokenAsync(tokenResult.RefreshToken)).ReturnsAsync(storedToken);
        _mockUserRepository.Setup(x => x.Get(_testUser.Id)).ReturnsAsync(_testUser);

        // Act
        var refreshed = await _tokenService.RefreshTokenAsync(tokenResult.RefreshToken);

        // Assert
        Assert.IsNotNull(refreshed.Token);
        Assert.IsNotNull(refreshed.RefreshToken);
        Assert.AreNotEqual(tokenResult.RefreshToken, refreshed.RefreshToken);
    }

    [Test]
    public void RefreshTokenAsync_InvalidToken_ThrowsUnauthorized()
    {
        // Arrange
        string invalidToken = "invalid_token";
        _mockUserRepository.Setup(x => x.GetRefreshTokenAsync(invalidToken)).ReturnsAsync((RefreshToken)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _tokenService.RefreshTokenAsync(invalidToken));
        Assert.That(ex.Message, Is.EqualTo("Invalid, expired, or revoked refresh token"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
