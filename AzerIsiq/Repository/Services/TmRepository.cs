using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class TmRepository : GenericRepository<Tm>, ITmRepository
{
    public TmRepository(AppDbContext context) : base(context)
    {
    }
}