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
    public async Task<IActionResult> Create([FromForm] SubstationDto dto)
    {
        try
        {
            var substation = await _substationService.CreateSubstationAsync(dto);
            return Ok( new { Message = "Success", Id = substation.Id, Name = substation.Name, Location = substation.Location });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Edit(int id, [FromForm] SubstationDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid substation data");

        var updatedSubstation = await _substationService.EditSubstationAsync(id, dto);
        return Ok(new { Message = "Success", Id = updatedSubstation.Id, Name = updatedSubstation.Name, DistrictId = updatedSubstation.DistrictId });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _substationService.DeleteSubstationAsync(id);
        return Ok(new { message = "Substation successfully deleted" });
    }
}
