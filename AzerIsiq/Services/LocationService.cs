using System.Globalization;
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

    public async Task<Location> CreateLocationAsync(string latitudeStr, string longitudeStr, string? address)
    {
        if (!decimal.TryParse(latitudeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var latitude) ||
            !decimal.TryParse(longitudeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var longitude))
        {
            throw new ArgumentException("Invalid latitude or longitude format");
        }

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