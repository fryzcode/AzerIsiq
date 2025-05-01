using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ICounterRepository : IGenericRepository<Counter>
{
    // Task<List<Counter>> GetAllBySubscriberIdAsync(int subscriberId);
    Task<List<Counter>> GetBySubscriberIdAsync(int subscriberId);

}