using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class SubstationRepository : GenericRepository<Substation>, ISubstationRepository
{
    private readonly AppDbContext _context;

    public SubstationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tm>> GetTmsBySubstationAsync(int substationId)
    {
        return await _context.Tms.Where(t => t.SubstationId == substationId).ToListAsync();
    }
}
