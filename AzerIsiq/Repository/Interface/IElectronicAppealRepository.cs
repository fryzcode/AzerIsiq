using AzerIsiq.Dtos;
using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IElectronicAppealRepository : IGenericRepository<ElectronicAppeal>
{
    Task<(IEnumerable<ElectronicAppeal> Items, int TotalCount)> GetPagedAsync(PagedRequestDto requestDto,
        ElectronicAppealFilterDto? filter);
}