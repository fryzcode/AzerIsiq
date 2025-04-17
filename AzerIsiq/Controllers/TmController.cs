using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AzerIsiq.Dtos;
using AzerIsiq.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TmController : ControllerBase
{
    private readonly ITmService _tmService;
    private readonly IValidator<TmDto> _tmDtoValidator;
    
    public TmController(ITmService tmService, IValidator<TmDto> tmDtoValidator, IValidator<TmDto> tmCreateValidator)
    {
        _tmService = tmService;
        _tmDtoValidator = tmDtoValidator;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetRegions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tmService.GetTmAsync(page, pageSize);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tm = await _tmService.GetTmByIdAsync(id);
        return Ok(new { Id = tm.Id, Name = tm.Name });
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] TmDto dto)
    {
        await _tmDtoValidator.ValidateAndThrowAsync(dto);
    
        var tm = await _tmService.CreateTmAsync(dto);
        return Ok(new { Message = "Success", Id = tm.Id, Name = tm.Name });
    }
    
    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [FromBody] TmDto dto)
    {
        await _tmDtoValidator.ValidateAndThrowAsync(dto);
        
        var updatedTm = await _tmService.EditTmAsync(id, dto);
        return Ok(new
        {
            Message = "Success",
            Id = updatedTm.Id,
            Name = updatedTm.Name,
            SubstationId = updatedTm.SubstationId
        });
    }
    
    [HttpGet("filtered")]
    public async Task<IActionResult> GetTmsByFilters(
        [FromQuery] PagedRequestDto request, 
        [FromQuery] int? regionId, 
        [FromQuery] int? districtId, 
        [FromQuery] int? substationId)
    {
        var result = await _tmService.GetTmsByFiltersAsync(request, regionId, districtId, substationId);
        return Ok(result);
    }
    
}