using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface ILocationRepository : IGenericRepository<Location>
{
    Task<Location?> GetByCoordinatesAsync(decimal latitude, decimal longitude);
}