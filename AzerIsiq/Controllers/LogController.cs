using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class LogController : ControllerBase
{
    private readonly ILoggingService _logService;

    public LogController(ILoggingService logService)
    {
        _logService = logService;
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetFiltered([FromQuery] LogEntryFilterDto filter)
    {
        var logs = await _logService.GetLogsAsync(filter);
        var totalCount = await _logService.CountLogsAsync(filter);
        return Ok(new { data = logs, total = totalCount });
    }

    [HttpGet("entities")]
    public async Task<IActionResult> GetEntity()
    {
        var entities = await _logService.GetAllEntityNamesAsync();
        return Ok(entities);
    }
    
    [HttpGet("by-subscriber-code/{subscriberCode}")]
    public async Task<IActionResult> GetLogsBySubscriberCode(string subscriberCode)
    {
        var logs = await _logService.GetLogsBySubscriberCodeAsync(subscriberCode);

        if (!logs.Any())
            return NotFound($"Not have log subscriberCode: {subscriberCode}");

        return Ok(logs);
    }

}