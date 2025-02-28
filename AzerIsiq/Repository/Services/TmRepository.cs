using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class TmRepository : GenericRepository<Tm>, ITmRepository
{
    private readonly ILoggerRepository _logger;

    public TmRepository(AppDbContext context, ILoggerRepository loggerRepository, IHttpContextAccessor httpContextAccessor) : base(context, loggerRepository, httpContextAccessor)
    {
        _logger = loggerRepository;
    }
}