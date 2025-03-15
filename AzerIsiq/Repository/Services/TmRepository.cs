using AzerIsiq.Data;
using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class TmRepository : GenericRepository<Tm>, ITmRepository
{
    public TmRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<PagedResultDto<Tm>> GetTmsByFiltersAsync(int? regionId, int? districtId, int? substationId, int page, int pageSize)
    {
        var query = _context.Tms
            .Include(t => t.Substation)
            .ThenInclude(s => s.District)
            .ThenInclude(d => d.Region)
            .AsQueryable();

        if (regionId.HasValue)
        {
            query = query.Where(t => t.Substation.District.RegionId == regionId);
        }

        if (districtId.HasValue)
        {
            query = query.Where(t => t.Substation.DistrictId == districtId);
        }

        if (substationId.HasValue)
        {
            query = query.Where(t => t.SubstationId == substationId);
        }

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<Tm>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}