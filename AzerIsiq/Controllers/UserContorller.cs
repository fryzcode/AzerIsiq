using System.Security.Claims;
using AzerIsiq.Dtos;
using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResultDto<UserListDto>>> GetUsers([FromQuery] UserQueryParameters parameters)
    {
        var result = await _userService.GetAllUsersAsync(parameters);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    
        var user = await _userService.GetUserByIdAsync(currentUserId);
        if (user == null)
            return NotFound();
    
        return Ok(user);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("block")]
    public async Task<IActionResult> BlockUser([FromBody] BlockUserDto dto)
    {
        await _userService.BlockUserAsync(dto.UserId, dto.IsBlocked);
        return Ok(new { message = $"User {(dto.IsBlocked ? "blocked" : "unblocked")}" });
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<ActionResult<RoleDto>> GetUserRoles()
    {
        var result = await _userService.GetAllRolesAsync();
        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("assign-roles")]
    public async Task<IActionResult> AssignRoles([FromBody] AssignRoleDto dto)
    {
        await _userService.AssignRolesToUserAsync(dto);
        return Ok(new { message = "Roles updated successfully" });
    }
}
