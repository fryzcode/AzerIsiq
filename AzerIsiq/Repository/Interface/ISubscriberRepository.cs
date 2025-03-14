using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ISubscriberRepository : IGenericRepository<Subscriber>
{
    Task<string> GenerateUniqueAtsAsync();
    Task<(List<Subscriber> Subscribers, int TotalCount)> GetSubscribersAsync(int page, int pageSize);
}