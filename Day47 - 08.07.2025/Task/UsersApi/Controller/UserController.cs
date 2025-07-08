using Microsoft.AspNetCore.Mvc;
using UserApi.Interfaces;
using UserApi.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest("User cannot be null");
        }
        var createdUser = await _userService.AddAsync(user);
        return Ok(createdUser);
    }
}
