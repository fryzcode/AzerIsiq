using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class RegionRepository : ReadOnlyRepository<Region>, IRegionRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<District>> GetDistrictsByRegionAsync(int regionId)
    {
        return await _context.Districts.Where(d => d.RegionId == regionId).ToListAsync();
    }
    
    public async Task<IReadOnlyList<Substation>> GetSubstationsByDistrictAsync(int districtId)
    {
        return await _context.Substations.Where(s => s.DistrictId == districtId).ToListAsync();
    }

    public async Task<IReadOnlyList<Tm>> GetTmsBySubstationAsync(int substationId)
    {
        return await _context.Tms.Where(t => t.SubstationId == substationId).ToListAsync();
    }

    public async Task<IReadOnlyList<Substation>> GetSubstationsByRegionAsync(int regionId)
    {
        var districts = await _context.Districts
            .Where(d => d.RegionId == regionId)
            .ToListAsync();

        var districtIds = districts.Select(d => d.Id).ToList();
        var substations = await _context.Substations
            .Where(s => districtIds.Contains(s.DistrictId))
            .ToListAsync();

        return substations;
    }

    public async Task<IReadOnlyList<Tm>> GetTmsByRegionAsync(int regionId)
    {
        return await _context.Tms
            .Include(tm => tm.Substation)
            .ThenInclude(substation => substation.District)
            .Where(tm => tm.Substation.District.RegionId == regionId)
            .ToListAsync();
    }

}