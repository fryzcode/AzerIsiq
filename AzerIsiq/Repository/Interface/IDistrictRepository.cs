using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IDistrictRepository : IReadOnlyRepository<District>
{
    Task<IReadOnlyList<Substation>> GetSubstationsByDistrictAsync(int districtId);
    Task<IReadOnlyList<Tm>> GetTmsByDistrictAsync(int districtId);
    Task<IReadOnlyList<Territory>> GetTerritoryByDistrictAsync(int districtId);
    Task<IReadOnlyList<Street>> GetStreetByTerritoryAsync(int territoryId);
}