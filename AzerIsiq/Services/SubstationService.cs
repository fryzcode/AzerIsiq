using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class SubstationService
{
    private readonly ISubstationRepository _substationRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ILocationService _locationService;

    public SubstationService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository,
        ILocationService locationService)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
        _locationService = locationService;
    }

    public async Task<Substation> CreateSubstationAsync(SubstationDto dto)
    {
        Location? location = null;

        if (dto.Longitude != 0 && dto.Latitude != 0)
        {
            location = await _locationService.CreateLocationAsync(
                dto.Latitude, 
                dto.Longitude, 
                dto.Address
            );
        }
        
        await ValidateRegionAndDistrictAsync(dto);

        var substation = new Substation
        {
            Name = dto.Name,
            DistrictId = dto.DistrictId,
            LocationId = location?.Id
        };

        await _substationRepository.CreateAsync(substation);
        return substation;
    }
    public async Task<Substation> EditSubstationAsync(int id, SubstationDto dto)
    {
        var substation = await _substationRepository.GetByIdAsync(id);
        if (substation == null)
            throw new Exception("Substation not found!");

        await ValidateRegionAndDistrictAsync(dto);

        Location? location = null;

        if (dto.Longitude != 0 && dto.Latitude != 0)
        {
            location = await _locationService.CreateLocationAsync(
                dto.Latitude, 
                dto.Longitude, 
                dto.Address
            );
            substation.LocationId = location.Id; 
        }

        substation.Name = dto.Name;
        substation.DistrictId = dto.DistrictId;

        await _substationRepository.UpdateAsync(substation);
        return substation;
    }
    public async Task<bool> DeleteSubstationAsync(int id)
    {
        var substation = await _substationRepository.GetByIdAsync(id);
        if (substation == null)
            throw new Exception("Substation not found!");

        await _substationRepository.DeleteAsync(substation.Id);
        return true;
    }
    private async Task ValidateRegionAndDistrictAsync(SubstationDto dto)
    {
        var region = await _regionRepository.GetByIdAsync(dto.RegionId);
        if (region == null)
            throw new Exception("Region not found!");

        var district = await _districtRepository.GetByIdAsync(dto.DistrictId);
        if (district == null || district.RegionId != dto.RegionId)
            throw new Exception("District not found or does not belong to the selected region");
    }
}