using AzerIsiq.Dtos;
using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ElectronicAppealController : ControllerBase
{
    private readonly IElectronicAppealService _electronicAppealService;
    
    public ElectronicAppealController(IElectronicAppealService electronicAppealService)
    {
        _electronicAppealService = electronicAppealService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Operator")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequestDto requestDto,
        [FromQuery] ElectronicAppealFilterDto? filter)
    {
        var result = await _electronicAppealService.GetAllAsync(requestDto, filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Operator")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _electronicAppealService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ElectronicAppealCreateDto dto)
    {
        var result = await _electronicAppealService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPatch("{id}/mark-as-read")]
    [Authorize(Roles = "Admin,Operator")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _electronicAppealService.MarkAsReadAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id}/mark-as-replied")]
    [Authorize(Roles = "Admin,Operator")]
    public async Task<IActionResult> MarkAsReplied(int id)
    {
        var result = await _electronicAppealService.MarkAsRepliedAsync(id);
        return Ok(result);
    }
    
    [HttpGet("statistics")]
    [Authorize(Roles = "Admin,Operator")]
    public async Task<IActionResult> GetStatistics()
    {
        var stats = await _electronicAppealService.GetStatisticsAsync();
        return Ok(stats);
    }

}