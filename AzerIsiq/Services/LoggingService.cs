using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Extensions.Repository;

public class LoggingService
{
    private readonly ILoggerRepository _logger;
    private readonly IAuthService _authService;

    public LoggingService(ILoggerRepository logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public async Task LogActionAsync(string action, string entityName, int entityId)
    {
        await _logger.LogAsync(new LogEntry()
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            UserId = _authService.GetCurrentUserId()
        });
    }
}