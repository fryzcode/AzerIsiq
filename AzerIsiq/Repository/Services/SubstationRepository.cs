using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class SubstationRepository : GenericRepository<Substation>, ISubstationRepository
{
    private readonly ILoggerRepository _logger;

    public SubstationRepository(AppDbContext context, ILoggerRepository loggerRepository, IHttpContextAccessor httpContextAccessor)
        : base(context, loggerRepository, httpContextAccessor)
    {
        _logger = loggerRepository;
    }

    public async Task<IEnumerable<Tm>> GetTmsBySubstationAsync(int substationId)
    {
        var tms = await _context.Tms.Where(t => t.SubstationId == substationId).ToListAsync();

        return tms;
    }
}
