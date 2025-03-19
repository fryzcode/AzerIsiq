using System.ComponentModel.DataAnnotations;
using AzerIsiq.Dtos;
using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;
using AzerIsiq.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriberController : ControllerBase
{
    private readonly ISubscriberService _subscriberService;
    private readonly IValidator<SubscriberRequestDto> _sbDtoValidator;
    
    public SubscriberController(ISubscriberService subscriberService,IValidator<SubscriberRequestDto> sbDtoValidator)
    {
        _subscriberService = subscriberService;
        _sbDtoValidator = sbDtoValidator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubscriberRequestDto dto)
    {
        await _sbDtoValidator.ValidateAndThrowAsync(dto);
        
        await _subscriberService.CreateSubscriberRequestAsync(dto);
        
        return Ok( new { Message = "Success" });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sb = await _subscriberService.GetSubscriberByIdAsync(id);
        return Ok(sb);
    }
    
    [HttpPost("sb-code")]
    public async Task<IActionResult> CreateSbCode(int id)
    {
        await _subscriberService.CreateSubscriberCodeAsync(id);
    
        return Ok(new { Message = "Success" });
    }
    
    [HttpPost("sb-counter")]
    public async Task<IActionResult> CreateSbCounter(int id, CounterDto dto)
    {
        await _subscriberService.CreateCounterForSubscriberAsync(id, dto);
    
        return Ok(new { Message = "Success" });
    }
    
    [HttpPost("sb-tm")]
    public async Task<IActionResult> ConnectSbToTm(int id, [FromBody] ConnectTmDto dto)
    {
        await _subscriberService.ConnectTmToSubscriberAsync(id, dto.TmId);

        return Ok(new { Message = "Success" });
    }
    
    [HttpGet("filtered")]
    public async Task<IActionResult> GetSubscriberByFilters(
        [FromQuery] PagedRequestDto request, [FromQuery] SubscriberFilterDto filter)
    {
        var result = await _subscriberService.GetSubscribersFilteredAsync(request, filter);
        return Ok(result);
    }
}
