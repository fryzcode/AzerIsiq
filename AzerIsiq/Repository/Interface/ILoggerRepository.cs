using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ILoggerRepository
{
    Task<IEnumerable<LogEntryDto>> GetFilteredAsync(LogEntryFilterDto filter);
    Task<int> CountFilteredAsync(LogEntryFilterDto filter);
    Task LogAsync(LogEntry logEntry);
}