using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Mvc;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RegionController : ControllerBase
{
    private readonly IRegionService _regionService;

    public RegionController(IRegionService regionService)
    {
        _regionService = regionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regions = await _regionService.GetAllAsync();
        return Ok(new { Message = "Success", Region = regions.Select(s => new { s.Id, s.Name }) });
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
        return region is not null ? Ok(new { Message = "Success", region.Id, region.Name }) : NotFound();
    }
    
    [HttpGet("{id}/districts")]
    public async Task<IActionResult> GetDistrictsByRegion(int id)
    {
        var districts = await _regionService.GetDistrictsByRegionAsync(id);
        return Ok(new { Message = "Success", Districts = districts.Select(d => new { d.Id, d.Name }) });
    }
    
    [HttpGet("district/{id}/substations")]
    public async Task<IActionResult> GetSubstationsByDistrict(int id)
    {
        var substations = await _regionService.GetSubstationByDistrictAsync(id);
        return Ok(new { Message = "Success", Substations = substations.Select(s => new { s.Id, s.Name }) });
    }
    
    [HttpGet("district/substation/{id}/tms")]
    public async Task<IActionResult> GetTmsBySubstation(int id)
    {
        var tms = await _regionService.GetTmsBySubstationAsync(id);
        return Ok(new { Message = "Success", Tms = tms.Select(s => new { s.Id, s.Name }) });
    }
    
    [HttpGet("{id}/tms")]
    public async Task<IActionResult> GetTmsByRegion(int id)
    {
        var tms = await _regionService.GetTmsByRegionAsync(id);
        return Ok(tms);
    }
}
