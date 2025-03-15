using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface ILocationService
{
    Task<Location> CreateLocationAsync(string latitudeStr, string longitudeStr, string? address);
    Task<bool> DeleteLocationAsync(int id);
    Task<Location> GetLocationByIdAsync(int id);
    Task<Location?> GetLocationByCoordinatesAsync(decimal latitude, decimal longitude);
}