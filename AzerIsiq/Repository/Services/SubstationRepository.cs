using AzerIsiq.Data;
using AzerIsiq.Dtos;
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

    public async Task<PagedResultDto<Substation>> GetSubstationsByFiltersAsync(int? regionId, int? districtId, int page, int pageSize)
    {
        
        var query = _context.Substations.Include(s=>s.Images).Include(s=>s.Location)
            .Include(s => s.District)
            .ThenInclude(d => d.Region)
            .AsQueryable();
        
        if (regionId.HasValue)
        {
            query = query.Where(s => s.District.RegionId == regionId);
        }

        if (districtId.HasValue)
        {
            query = query.Where(s => s.DistrictId == districtId);
        }
        
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PagedResultDto<Substation>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
