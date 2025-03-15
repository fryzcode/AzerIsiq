using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ITmRepository : IGenericRepository<Tm>
{
    Task<PagedResultDto<Tm>> GetTmsByFiltersAsync(int? regionId, int? districtId, int? substationId, int page, int pageSize);
}