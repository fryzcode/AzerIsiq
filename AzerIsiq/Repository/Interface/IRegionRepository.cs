using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IRegionRepository : IReadOnlyRepository<Region>
{
    Task<IReadOnlyList<District>> GetDistrictsByRegionAsync(int regionId);
    Task<IReadOnlyList<Substation>> GetSubstationsByDistrictAsync(int districtId);
    Task<IReadOnlyList<Tm>> GetTmsBySubstationAsync(int substationId);
    Task<IReadOnlyList<Substation>> GetSubstationsByRegionAsync(int regionId);
    Task<IReadOnlyList<Tm>> GetTmsByRegionAsync(int regionId);
}
