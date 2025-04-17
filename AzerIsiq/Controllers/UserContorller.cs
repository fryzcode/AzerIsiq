using AzerIsiq.Dtos;
using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<UserListDto>>> GetUsers([FromQuery] UserQueryParameters parameters)
    {
        var result = await _userService.GetAllUsersAsync(parameters);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }
    
    [HttpPost("block")]
    public async Task<IActionResult> BlockUser([FromBody] BlockUserDto dto)
    {
        await _userService.BlockUserAsync(dto.UserId, dto.IsBlocked);
        return Ok(new { message = $"User {(dto.IsBlocked ? "blocked" : "unblocked")}" });
    }
    
    [HttpGet("roles")]
    public async Task<ActionResult<RoleDto>> GetUserRoles()
    {
        var result = await _userService.GetAllRolesAsync();
        return Ok(result);
    }
}
