using AzerIsiq.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        return Ok(districts);
    }
    
    [HttpGet("{id}/substations")]
    public async Task<IActionResult> GetSubstationsByRegion(int id)
    {
        var substations = await _regionService.GetSubstationsByRegionAsync(id);
        return Ok(substations);
    }
}
