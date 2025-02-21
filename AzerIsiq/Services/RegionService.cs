using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class RegionService : ReadOnlyService<Region>
{
    public RegionService(IReadOnlyRepository<Region> repository) : base(repository) { }
}