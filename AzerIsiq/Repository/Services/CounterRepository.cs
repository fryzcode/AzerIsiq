using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class CounterRepository: GenericRepository<Counter>, ICounterRepository
{
    private readonly AppDbContext _context;
    public CounterRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}