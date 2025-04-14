using System.ComponentModel.DataAnnotations;
using AzerIsiq.Dtos;
using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;
using AzerIsiq.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var sb = await _subscriberService.GetSubscriberByIdAsync(id);
        return Ok(sb);
    }
    
    [HttpPost("sb-code")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSbCode([FromQuery] int id)
    {
        await _subscriberService.CreateSubscriberCodeAsync(id);
    
        return Ok(new { Message = "Success" });
    }
    
    [HttpPost("sb-counter")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSbCounter ([FromQuery] int id, CounterDto dto)
    {
        await _subscriberService.CreateCounterForSubscriberAsync(id, dto);
    
        return Ok(new { Message = "Success" });
    }
    
    [HttpPost("sb-tm")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConnectSbToTm(int id, [FromBody] ConnectTmDto dto)
    {
        await _subscriberService.ConnectTmToSubscriberAsync(id, dto.TmId);

        return Ok(new { Message = "Success" });
    }
    
    [HttpPost("sb-apply")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApplySubscriberContract(int id)
    {
        var (isConfirmed, subscriber) = await _subscriberService.ApplySubscriberContractAsync(id);

        if (isConfirmed)
        {
            return Ok(new { Message = "Subscriber is already confirmed"});
        }

        return Ok(new { Message = "Subscriber successfully confirmed"});
    }
    [HttpGet("filtered")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSubscriberByFilters(
        [FromQuery] PagedRequestDto request, [FromQuery] SubscriberFilterDto filter)
    {
        var result = await _subscriberService.GetSubscribersFilteredAsync(request, filter);
        return Ok(result);
    }
}
