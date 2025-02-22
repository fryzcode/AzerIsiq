using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IRegionRepository : IReadOnlyRepository<Region>
{
    Task<IEnumerable<District>> GetDistrictsByRegionAsync(int regionId);
    Task<IEnumerable<Substation>> GetSubstationsByRegionAsync(int regionId);
}
