using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class LoggerRepository : ILoggerRepository
{
    private readonly AppDbContext _context;

    public LoggerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(LogEntry logEntry)
    {
        await _context.LogEntries.AddAsync(logEntry);
        await _context.SaveChangesAsync();
    }
}