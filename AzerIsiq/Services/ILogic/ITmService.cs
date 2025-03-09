using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface ITmService
{
    Task<Tm> GetTmByIdAsync(int id);
    Task<PagedResultDto<TmResponeDto>> GetTmAsync(int page, int pageSize);
    Task<Tm> CreateTmAsync(TmDto dto);
    Task<Tm> EditTmAsync(int id, TmDto dto);
    Task ValidateTmDataAsync(TmDto dto);
}