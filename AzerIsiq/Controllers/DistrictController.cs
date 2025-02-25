using AzerIsiq.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DistrictController : ControllerBase
{
    private readonly DistrictService _districtService;

    public DistrictController(DistrictService districtService)
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
}
