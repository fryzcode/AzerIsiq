using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface IRegionService: IReadOnlyService<Region>
{
    Task<IEnumerable<DistrictDto>> GetDistrictsByRegionAsync(int regionId);
    Task<IEnumerable<SubstationDto>> GetSubstationByDistrictAsync(int regionId);
    Task<IEnumerable<SubstationDto>> GetSubstationsByRegionAsync(int regionId);
    Task<PagedResultDto<RegionDto>> GetRegionAsync(int page, int pageSize);
    Task<IEnumerable<TmDto>> GetTmsBySubstationAsync(int substationId);
    Task<IEnumerable<TmDto>> GetTmsByRegionAsync(int regionId);
}