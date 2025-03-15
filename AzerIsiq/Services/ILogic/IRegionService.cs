using AzerIsiq.Dtos;

namespace AzerIsiq.Services.ILogic;

public interface IRegionService
{
    Task<IEnumerable<DistrictDto>> GetDistrictsByRegionAsync(int regionId);
    Task<IEnumerable<SubstationDto>> GetSubstationByDistrictAsync(int regionId);
    Task<IEnumerable<SubstationDto>> GetSubstationsByRegionAsync(int regionId);
    Task<PagedResultDto<RegionDto>> GetRegionAsync(int page, int pageSize);
    Task<IEnumerable<TmDto>> GetTmsBySubstationAsync(int substationId);
}