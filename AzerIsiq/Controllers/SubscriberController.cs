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
    
    [HttpGet]
    public async Task<IActionResult> GetPagedSubscriber(int page = 1, int pageSize = 10)
    {
        var result = await _subscriberService.GetSubscribersAsync(page, pageSize);
        return Ok(result);
    }
}
