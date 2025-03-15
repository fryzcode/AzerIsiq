using System.Linq.Expressions;
using AzerIsiq.Data;
using AzerIsiq.Dtos;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public ReadOnlyRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<PagedResultDto<T>> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<PagedResultDto<T>> GetPageAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        int totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}