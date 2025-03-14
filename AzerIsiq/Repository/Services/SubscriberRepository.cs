using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class SubscriberRepository : GenericRepository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public async Task<(List<Subscriber> Subscribers, int TotalCount)> GetSubscribersAsync(int page, int pageSize)
    {
        var query = _context.Subscribers
            .Include(s => s.Region)
            .Include(s => s.District)
            .OrderBy(s => s.Id);

        int totalCount = await query.CountAsync();

        var subscribers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (subscribers, totalCount);
    }
    
    public async Task<string> GenerateUniqueAtsAsync()
    {
        var random = new Random();
        string atsCode;
        bool exists;

        do
        {
            atsCode = "ATS" + string.Concat(Enumerable.Range(0, 15).Select(_ => random.Next(0, 10)));
            exists = await _context.Set<Subscriber>().AnyAsync(s => s.Ats == atsCode);
        }
        while (exists);

        return atsCode;
    }
}