using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class DistrictRepository : ReadOnlyRepository<District>, IDistrictRepository
{
    private readonly AppDbContext _context;

    public DistrictRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Substation>> GetSubstationsByDistrictAsync(int districtId)
    {
        return await _context.Substations.Where(s => s.DistrictId == districtId).ToListAsync();
    }
}
