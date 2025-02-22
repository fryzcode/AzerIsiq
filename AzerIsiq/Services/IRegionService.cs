using AzerIsiq.Dtos;

namespace AzerIsiq.Services;

public interface IRegionService
{
    Task<IEnumerable<DistrictDto>> GetDistrictsByRegionAsync(int regionId);
    Task<IEnumerable<SubstationDto>> GetSubstationsByRegionAsync(int regionId);

}