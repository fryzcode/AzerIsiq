using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class SubstationRepository : GenericRepository<Substation>, ISubstationRepository
{
    public SubstationRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Tm>> GetTmsBySubstationAsync(int substationId)
    {
        var tms = await _context.Tms.Where(t => t.SubstationId == substationId).ToListAsync();

        return tms;
    }
    
    public async Task<Substation?> GetByIdWithIncludesAsync(int id)
    {
        return await _context.Substations
            .Include(s => s.District)
            .ThenInclude(d => d.Region)
            .Include(s => s.Location)
            .Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

}
