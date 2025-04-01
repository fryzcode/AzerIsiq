using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface IDistrictService : IReadOnlyService<District>
{
    Task<IEnumerable<SubstationDto>> GetSubstationsByDistrictAsync(int districtId);
    Task<IEnumerable<TerritoryDto>> GetTerritoryByDistrictAsync(int districtId);
    Task<IEnumerable<TmDto>> GetTmsByDistrictAsync(int districtId);
    Task<IEnumerable<StreetDto>> GetStreetByTerritoryAsync(int territiryId);
}