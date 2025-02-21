using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class RegionRepository : ReadOnlyRepository<Region>
{
    public RegionRepository(AppDbContext context) : base(context) { }
}