using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface ILocationService
{
    Task<Location> CreateLocationAsync(string latitudeStr, string longitudeStr, string? address);
    Task<Location?> GetLocationByCoordinatesAsync(decimal latitude, decimal longitude);
}