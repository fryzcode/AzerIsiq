using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}