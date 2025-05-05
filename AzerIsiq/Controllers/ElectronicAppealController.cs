using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Services.ILogic;
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
    public async Task<IActionResult> GetAll()
    {
        var result = await _electronicAppealService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _electronicAppealService.MarkAsReadAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id}/mark-as-replied")]
    public async Task<IActionResult> MarkAsReplied(int id)
    {
        var result = await _electronicAppealService.MarkAsRepliedAsync(id);
        return Ok(result);
    }
}