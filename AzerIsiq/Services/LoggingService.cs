using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AzerIsiq.Extensions.Repository;

public class LoggingService:ILoggingService
{
    private readonly ILoggerRepository _loggerRepository;
    private readonly IAuthService _authService;

    public LoggingService(ILoggerRepository loggerRepository, IAuthService authService)
    {
        _loggerRepository = loggerRepository;
        _authService = authService;
    }
    
    public async Task<IEnumerable<LogEntryDto>> GetLogsAsync(LogEntryFilterDto filter)
    {
        return await _loggerRepository.GetFilteredAsync(filter);
    }
    
    public async Task<int> CountLogsAsync(LogEntryFilterDto filter)
    {
        return await _loggerRepository.CountFilteredAsync(filter);
    }
    
    public async Task LogActionAsync(string action, string entityType, int entityId, string entityName)
    {
        var logEntry = new LogEntry
        {
            Action = action,
            EntityId = entityId,
            EntityType = entityType,
            EntityName = entityName,
            UserId = _authService.GetCurrentUserId(),
            Timestamp = DateTime.UtcNow
        };

        await _loggerRepository.LogAsync(logEntry);
    }
    public async Task<IEnumerable<string>> GetAllEntityNamesAsync()
    {
        return await _loggerRepository.GetAllEntityNamesAsync();
    }
    
    public async Task<IEnumerable<LogEntryDto>> GetLogsBySubscriberCodeAsync(string subscriberCode)
    {
        return await _loggerRepository.GetLogsBySubscriberCodeAsync(subscriberCode);
    }
}