using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface ITmService
{
    Task<Tm> GetTmByIdAsync(int id);
    Task<PagedResultDto<TmDtoPaged>> GetTmsByFiltersAsync(PagedRequestDto request, int? regionId, int? districtId, int? substationId);
    Task<PagedResultDto<TmResponeDto>> GetTmAsync(int page, int pageSize);
    Task<Tm> CreateTmAsync(TmDto dto);
    Task<Tm> EditTmAsync(int id, TmDto dto);
    Task<bool> DeleteTmAsync(int id);
    Task ValidateTmDataAsync(TmDto dto);
}