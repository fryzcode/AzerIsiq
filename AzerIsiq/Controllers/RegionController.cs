using AzerIsiq.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize(Roles = "Admin")]
public class RegionController : ControllerBase
{
    private readonly RegionService _regionService;

    public RegionController(RegionService regionService)
    {
        _regionService = regionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _regionService.GetAllAsync());
    }
    
    [HttpGet("paged")]
    public async Task<IActionResult> GetRegions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _regionService.GetRegionAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var region = await _regionService.GetByIdAsync(id);
        return region is not null ? Ok(region) : NotFound();
    }
    
    [HttpGet("{id}/districts")]
    public async Task<IActionResult> GetDistrictsByRegion(int id)
    {
        var districts = await _regionService.GetDistrictsByRegionAsync(id);
        return Ok(new { Message = "Success", Districts = districts.Select(d => new { d.Id, d.Name }) });
    }
    
    [HttpGet("{id}/substations")]
    public async Task<IActionResult> GetSubstationsByRegion(int id)
    {
        var substations = await _regionService.GetSubstationsByRegionAsync(id);
        return Ok(substations);
    }
}
