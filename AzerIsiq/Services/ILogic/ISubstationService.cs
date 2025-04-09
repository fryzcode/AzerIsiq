using AzerIsiq.Dtos;
using AzerIsiq.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace AzerIsiq.Services;

public interface ISubstationService
{
    Task<PagedResultDto<SubstationResponseDto>> GetSubstationAsync(int page, int pageSize);
    Task<Substation> CreateSubstationAsync(SubstationDto dto);
    Task<Substation> EditSubstationAsync(int id, SubstationDto dto);
    Task<bool> DeleteSubstationAsync(int id);
    Task<SubstationGetDto> GetSubstationByIdAsync(int id);
    Task ValidateRegionAndDistrictAsync(SubstationDto dto);
    Task<PagedResultDto<SubstationDto>> GetSubstationByDistrictAsync(PagedRequestDto request, int districtId);
    Task<PagedResultDto<SubstationDto>> GetSubstationsByFiltersAsync(PagedRequestDto request, int? regionId, int? districtId);
}