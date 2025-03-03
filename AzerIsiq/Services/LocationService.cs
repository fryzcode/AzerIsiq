using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<Location> CreateLocationAsync(decimal latitude, decimal longitude, string? address)
    {
        var existingLocation = await _locationRepository.GetByCoordinatesAsync(latitude, longitude);
        if (existingLocation != null)
        {
            return existingLocation;
        }

        var location = new Location
        {
            Latitude = latitude,
            Longitude = longitude,
            Address = address ?? ""
        };
        
        await _locationRepository.CreateAsync(location);
        return location;
    }

    public async Task<Location?> GetLocationByCoordinatesAsync(decimal latitude, decimal longitude)
    {
        return await _locationRepository.GetByCoordinatesAsync(latitude, longitude);
    }
}