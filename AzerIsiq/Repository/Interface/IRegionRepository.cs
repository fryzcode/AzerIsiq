using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IRegionRepository : IReadOnlyRepository<Region>
{
    Task<IEnumerable<District>> GetDistrictsByRegionAsync(int regionId);
    Task<IEnumerable<Substation>> GetSubstationsByDistrictAsync(int districtId);
    Task<IEnumerable<Tm>> GetTmsBySubstationAsync(int substationId);
    Task<IEnumerable<Substation>> GetSubstationsByRegionAsync(int regionId);
    Task<IEnumerable<Tm>> GetTmsByRegionAsync(int regionId);
}
