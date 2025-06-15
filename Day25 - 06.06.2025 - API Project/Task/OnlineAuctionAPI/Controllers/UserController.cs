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
    [Authorize(Roles = "Seller,Bidder,Admin")]
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
    [Authorize(Roles = "Seller,Bidder,Admin")]
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

    [HttpPatch("change-password")]
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

    [HttpGet("GetWalletByUserId")]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult<VirtualWalletResponseDto>> GetUserWithWalletByUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("UserId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogWarning("Invalid or missing userId in token");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Please provide a valid userId.",
                Data = null
            });
        }

        var user = await _userService.GetUserWithWalletByUserIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("No user found with User ID {UserId}", userId);
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "No user found with the given User Id",
                Data = null
            });
        }
        if (user.VirtualWallet == null)
        {
            _logger.LogWarning("No virtual wallet found for user {UserId}", userId);
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "No virtual wallet found for the given User Id",
                Data = null
            });
        }
        _logger.LogInformation("User with User ID {UserId} retrieved successfully", userId);
        return Ok(new ApiResponse<VirtualWalletResponseDto>
        {
            Success = true,
            Message = "User with wallet retrieved successfully",
            Data = new VirtualWalletResponseDto
            {
                Id = user.VirtualWallet.Id,
                UserId = user.Id,
                Balance = user.VirtualWallet.Balance,
            }
        });
    }

    [HttpPost("AddVirtualWalletToUser")]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult<VirtualWalletResponseDto>> AddVirtualWalletToUser([FromBody] VirtualWalletAddDto virtualWallet)
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("UserId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            _logger.LogWarning("Invalid or missing userId in token");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Please provide a valid userId.",
                Data = null
            });
        }

        var user = await _userService.AddVirtualWalletToUserAsync(userId, virtualWallet);

        _logger.LogInformation("Virtual wallet added to user with ID {UserId} successfully", userId);

        var wallet = user.VirtualWallet!;
        var walletDto = new VirtualWalletResponseDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance,
            UpdatedAt = wallet.UpdatedAt
        };

        return Ok(new ApiResponse<VirtualWalletResponseDto>
        {
            Success = true,
            Message = "Virtual wallet added successfully",
            Data = walletDto
        });
    }

    [HttpPatch("AddFundsToWallet")]
    [Authorize(Roles = "Bidder")]
    public async Task<ActionResult<VirtualWalletResponseDto>> AddFundsToWallet([FromQuery] decimal amount)
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("UserId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Please provide a valid userId.",
                Data = null
            });
        }

        var user = await _userService.AddFundsToVirtualWalletAsync(userId, amount);
        var wallet = user.VirtualWallet!;
        var walletDto = new VirtualWalletResponseDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance,
            UpdatedAt = wallet.UpdatedAt
        };

        return Ok(new ApiResponse<VirtualWalletResponseDto>
        {
            Success = true,
            Message = "Funds added successfully",
            Data = walletDto
        });
    }
}