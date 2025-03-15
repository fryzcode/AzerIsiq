using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DistrictController : ControllerBase
{
    private readonly IDistrictService _districtService;

    public DistrictController(IDistrictService districtService)
    {
        _districtService = districtService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _districtService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var region = await _districtService.GetByIdAsync(id);
        return region is not null ? Ok(region) : NotFound();
    }
    
    [HttpGet("{id}/substations")]
    public async Task<IActionResult> GetSubstationsByDistrict(int id)
    {
        var substations = await _districtService.GetSubstationsByDistrictAsync(id);
        return Ok(new { Message = "Success", Districts = substations.Select(s => new { s.Id, s.Name }) });
    }
    
}
