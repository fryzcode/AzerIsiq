using AzerIsiq.Dtos;
using AzerIsiq.Services;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
// [Authorize(Roles = "Admin")]
public class SubstationController : ControllerBase
{
    private readonly ISubstationService _substationService;

    public SubstationController(ISubstationService substationService)
    {
        _substationService = substationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] SubstationDto dto)
    {
        var substation = await _substationService.CreateSubstationAsync(dto);
        return Ok( new { Message = "Success", Id = substation.Id, Name = substation.Name, Location = substation.Location });
    }
    
    [HttpGet("paged")]
    public async Task<IActionResult> GetSubstations([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _substationService.GetSubstationAsync(page, pageSize);
        return Ok(result);
    }
    
    [HttpGet("by-district/{districtId}")]
    public async Task<IActionResult> GetSubstationsByDistrict(int districtId, [FromQuery] PagedRequestDto request)
    {
        var result = await _substationService.GetSubstationByDistrictAsync(request, districtId);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sb = await _substationService.GetSubstationByIdAsync(id);
        
        return Ok(sb);
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
