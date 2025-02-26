using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AzerIsiq.Repository.Services;

public class GenericRepository<T> : ReadOnlyRepository<T>, IGenericRepository<T> where T : class
{
    protected readonly ILoggerRepository _logger;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    public GenericRepository(AppDbContext context, ILoggerRepository loggerRepository, IHttpContextAccessor httpContextAccessor)
        : base(context)
    {
        _logger = loggerRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        await _logger.LogAsync(new LogEntry
        {
            Action = "Create",
            EntityName = typeof(T).Name,
            EntityId = GetEntityId(entity),
            UserId = GetCurrentUserId()
        });

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        await _logger.LogAsync(new LogEntry
        {
            Action = "Update",
            EntityName = typeof(T).Name,
            EntityId = GetEntityId(entity),
            UserId = GetCurrentUserId()
        });
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync(new LogEntry
            {
                Action = "Delete",
                EntityName = typeof(T).Name,
                EntityId = id,
                UserId = GetCurrentUserId()
            });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }
    private int GetEntityId(T entity)
    {
        var property = typeof(T).GetProperty("Id");
        return property != null ? (int)property.GetValue(entity) : 0;
    }
}
