using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ISubstationRepository : IGenericRepository<Substation>
{
    Task<IEnumerable<Tm>> GetTmsBySubstationAsync(int substationId);
    Task<Substation?> GetByIdWithIncludesAsync(int id);
    Task<PagedResultDto<Substation>> GetSubstationsByFiltersAsync(int? regionId, int? districtId, int page, int pageSize);
}
