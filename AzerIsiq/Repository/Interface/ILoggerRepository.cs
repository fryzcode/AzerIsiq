using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ILoggerRepository
{
    Task LogAsync(LogEntry logEntry);
}