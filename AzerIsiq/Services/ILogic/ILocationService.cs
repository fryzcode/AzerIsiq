using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface ILocationService
{
    Task<Location> CreateLocationAsync(decimal latitude, decimal longitude, string? address);
    Task<Location?> GetLocationByCoordinatesAsync(decimal latitude, decimal longitude);
}