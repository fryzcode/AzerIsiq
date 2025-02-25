using AzerIsiq.Dtos;
using AzerIsiq.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubstationController : ControllerBase
{
    private readonly SubstationService _substationService;

    public SubstationController(SubstationService substationService)
    {
        _substationService = substationService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] SubstationDto dto)
    {
        try
        {
            var substation = await _substationService.CreateSubstationAsync(dto);
            return CreatedAtAction(nameof(Add), new { id = substation.Id }, substation);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
