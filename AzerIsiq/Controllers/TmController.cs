using System.ComponentModel.DataAnnotations;
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
// [Authorize(Roles = "Admin")]
public class TmController : ControllerBase
{
    private readonly TmService _tmService;
    private readonly IValidator<TmDto> _tmDtoValidator;

    public TmController(TmService tmService, IValidator<TmDto> tmDtoValidator)
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
        return tm is not null ? Ok(new {Id = tm.Id, Name = tm.Name}) : NotFound(new {Message = "Tm not found!"});
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TmDto dto)
    {
        try
        {
            ValidationResult validationResult = await _tmDtoValidator.ValidateAsync(dto);
                
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
            
            var tm = await _tmService.CreateTmAsync(dto);
            return Ok( new { Message = "Success", Id = tm.Id, Name = tm.Name });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, [FromBody] TmDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid tm data");

        var updatedTm = await _tmService.EditTmAsync(id, dto);
        return Ok(new { Message = "Success", Id = updatedTm.Id, Name = updatedTm.Name, SubstationId = updatedTm.SubstationId });
    }
    
}