using AzerIsiq.Dtos;
using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}
