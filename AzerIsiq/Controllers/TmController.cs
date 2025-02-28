using AzerIsiq.Dtos;
using AzerIsiq.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TmController : ControllerBase
{
    private readonly TmService _tmService;

    public TmController(TmService tmService)
    {
        _tmService = tmService;
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