using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class CounterRepository: GenericRepository<Counter>, ICounterRepository
{
    private readonly AppDbContext _context;
    public CounterRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Counter>> GetBySubscriberIdAsync(int subscriberId)
    {
        return await _context.Counters
            .Where(c => c.SubscriberId == subscriberId)
            .ToListAsync();
    }
}