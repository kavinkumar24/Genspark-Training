using Microsoft.AspNetCore.Mvc;
using Notify.Models;
using Notify.Interfaces;
using Notify.Models.DTO;
using Notify.Misc;
using Microsoft.AspNetCore.Authorization;

namespace Notify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    [Authorize]
    [CustomExceptionFilter]
    public async Task<ActionResult<User?>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpGet]
    [Authorize]
    [CustomExceptionFilter]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    [Authorize]
    [CustomExceptionFilter]
    public async Task<ActionResult> AddUser([FromBody] UserRegisterdto user)
    {
        await _userService.AddUserAsync(user);
        return Ok(new { message = "User created successfully" });
    }

    [HttpPut("{id}")]
    [Authorize]
    [CustomExceptionFilter]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest("User ID mismatch");
        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    [CustomExceptionFilter]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}