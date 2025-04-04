using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ISubscriberRepository : IGenericRepository<Subscriber>
{
    Task<string> GenerateUniqueAtsAsync();
    Task<PagedResultDto<SubscriberDto>> GetSubscriberByFiltersAsync(SubscriberFilterDto dto);
    Task<bool> ExistsBySubscriberCodeAsync(string subscriberCode);
    public Task<bool> ExistsBySubscriberFinAsync(string finCode);
}