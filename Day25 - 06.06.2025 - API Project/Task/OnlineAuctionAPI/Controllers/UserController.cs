using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Models.DTO;

namespace OnlineAuctionAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        _logger.LogInformation("Attempted to retrieve user with ID {UserId}", id);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", id);
            return NotFound("User not found");
        }
        _logger.LogInformation("User with ID {UserId} retrieved successfully", id);
        return Ok(new ApiResponse<User>
        {
            Success = true,
            Message = "User retrieved successfully",
            Data = user
        });
    }

    [HttpGet("GetByEmail")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult> GetByEmail([FromQuery] string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        _logger.LogInformation("Attempted to retrieve user with email {Email}", email);
        return Ok(new ApiResponse<User>
        {
            Success = true,
            Message = "User retrieved successfully",
            Data = user
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> CreateUser([FromBody] UserRegisterRequestDto userDto)
    {
        var newUser = await _userService.CreateUserAsync(userDto);
        _logger.LogInformation("User created successfully with ID {UserId}", newUser.Id);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, new ApiResponse<User>
        {
            Success = true,
            Message = "User created successfully",
            Data = newUser
        });
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var user = await _userService.DeleteUserAsync(id);
        _logger.LogInformation("User with ID {UserId} deleted successfully", id);
        return Ok(new ApiResponse<User>
        {
            Success = true,
            Message = "User deleted successfully",
            Data = user
        });
    }

    [HttpPut("UpdateUser")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<ActionResult> Updateuser([FromQuery] Guid id, [FromBody] UserUpdateRequestDto updatedto)
    {
        var user = await _userService.UpdateUserInfoAsync(id, updatedto);
        _logger.LogInformation("User with ID {UserId} updated successfully", id);
        return Ok(new ApiResponse<User>
        {
            Success = true,
            Message = "User updated successfully",
            Data = user
        });
    }

    [HttpPost("change-password")]
    [Authorize(Roles = "Seller,Bidder")]
    public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] ChangePasswordRequestDto dto)
    {
        
        await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
        _logger.LogInformation("Password changed successfully for user {UserId}", userId);
        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Password changed successfully",
            Data = null
        });
    }
}