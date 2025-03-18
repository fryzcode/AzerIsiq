using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Location?> GetByCoordinatesAsync(decimal latitude, decimal longitude)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);
    }
}
