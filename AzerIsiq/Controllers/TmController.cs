using AzerIsiq.Dtos;
using AzerIsiq.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class TmController : ControllerBase
{
    private readonly TmService _tmService;

    public TmController(TmService tmService)
    {
        _tmService = tmService;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetRegions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tmService.GetTmAsync(page, pageSize);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TmDto dto)
    {
        try
        {
            var tm = await _tmService.CreateTmAsync(dto);
            return Ok( new { Message = "Success", Id = tm.Id, Name = tm.Name });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}