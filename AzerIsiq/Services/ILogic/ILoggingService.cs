using AzerIsiq.Dtos.LogEntryDto;

namespace AzerIsiq.Services.ILogic;

public interface ILoggingService
{
    Task<IEnumerable<LogEntryDto>> GetLogsAsync(LogEntryFilterDto filter);
    Task<int> CountLogsAsync(LogEntryFilterDto filter);
    Task LogActionAsync(string action, string entityName, int entityId);
    Task<IEnumerable<string>> GetAllEntityNamesAsync();
    Task<IEnumerable<LogEntryDto>> GetLogsBySubscriberCodeAsync(string subscriberCode);

}