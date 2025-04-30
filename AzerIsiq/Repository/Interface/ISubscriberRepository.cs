using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ISubscriberRepository : IGenericRepository<Subscriber>
{
    Task<string> GenerateUniqueAtsAsync();
    Task<PagedResultDto<SubscriberDto>> GetSubscriberByFiltersAsync(SubscriberFilterDto dto);
    Task<bool> ExistsBySubscriberCodeAsync(string subscriberCode);
    public Task<bool> ExistsBySubscriberFinAsync(string finCode);
    Task<List<Subscriber>> GetByUserIdAsync(int userId);
    Task<List<Subscriber>> GetRequestsByFinAsync(string finCode);
    Task<List<Subscriber>> GetUserRequestsInLastMonthAsync(int userId);
    Task<Subscriber?> GetWithCountersByCodeAsync(string subscriberCode);
}