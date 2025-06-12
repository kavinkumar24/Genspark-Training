using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;
using OnlineAuctionAPI.Service;

namespace OnlineAuctionAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthService authenticationService, ITokenService tokenService, ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponseDto>> UserLogin(UserLoginRequestDto loginRequest)
    {
        var result = await _authenticationService.LoginAsync(loginRequest);
        _logger.LogInformation("User {Email} logged in successfully", loginRequest.Email);
        return Ok(new ApiResponse<UserLoginResponseDto>
        {
            Success = true,
            Message = "Login successful",
            Data = result
        });
    }

    [HttpPost("logout")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult> Logout([FromBody] string refreshToken)
    {
       await _authenticationService.LogoutAsync(refreshToken);
        _logger.LogInformation("Logout successful for refresh token {RefreshToken}", refreshToken);
        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Logout successful",
            Data = null
        });
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<UserLoginResponseDto>> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _tokenService.RefreshTokenAsync(refreshToken);
        if (result == null)
        {
            _logger.LogWarning("Invalid refresh token attempted: {RefreshToken}", refreshToken);
            return Unauthorized("Invalid refresh token");
        }
        _logger.LogInformation("Token refreshed successfully for refresh token {RefreshToken}", refreshToken);
        return Ok(new ApiResponse<UserLoginResponseDto>
        {
            Success = true,
            Message = "Token refreshed successfully",
            Data = result
        });
    }

    [HttpGet("me")]
    [Authorize(Roles = "Admin,Seller,Bidder")]
    public async Task<ActionResult<User>> GetMe()
    {
        var user = await _authenticationService.GetCurrentUserAsync(User);
        return Ok(new ApiResponse<User>
        {
            Success = true,
            Message = "User profile retrieved successfully",
            Data = user
        });
    }
}