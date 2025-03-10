using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class SubstationService : ISubstationService
{
    private readonly ISubstationRepository _substationRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ILocationService _locationService;
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;

    public SubstationService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository,
        IImageRepository imageRepository,
        ILocationService locationService,
        IImageService imageService)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
        _imageRepository = imageRepository;
        _locationService = locationService;
        _imageService = imageService;
    }

    public async Task<Substation> CreateSubstationAsync(SubstationDto dto)
    {
        Location? location = null;

        if (!string.IsNullOrEmpty(dto.Longitude) && !string.IsNullOrEmpty(dto.Latitude))
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
    
        var createdSubstation = await _substationRepository.CreateAsync(substation);

        if (dto.Image != null)
        {
            var image = await _imageService.UploadImageAsync(dto.Image);
            image.SubstationId = createdSubstation.Id;
        
            await _imageService.UpdateSubOrTmImageAsync(image);
        }

        return createdSubstation;
    }
    public async Task<Substation> EditSubstationAsync(int id, SubstationDto dto)
    {
        var substation = await _substationRepository.GetByIdAsync(id);
        if (substation == null)
            throw new NotFoundException($"No districts found for region ID {id}.");

        if (dto.RegionId > 0 && dto.DistrictId > 0)
        {
            await ValidateRegionAndDistrictAsync(dto);
        }

        if (!string.IsNullOrEmpty(dto.Longitude) && !string.IsNullOrEmpty(dto.Latitude))
        {
            var location = await _locationService.CreateLocationAsync(dto.Latitude, dto.Longitude, dto.Address);
            substation.LocationId = location.Id;
        }

        if (!string.IsNullOrEmpty(dto.Name))
            substation.Name = dto.Name;

        if (dto.DistrictId > 0)
            substation.DistrictId = dto.DistrictId;
        
        await _substationRepository.UpdateAsync(substation);

        if (dto.Image != null)
        {
            var existingImage = await _imageService.GetImageBySubstationIdAsync(substation.Id);
            if (existingImage != null)
            {
                var updateDto = new ImageUpdateDto
                {
                    Id = existingImage.Id,
                    File = dto.Image,
                    SubstationId = substation.Id
                };
                await _imageService.UpdateImageAsync(updateDto);
            }
            else
            {
                var image = await _imageService.UploadImageAsync(dto.Image);
                image.SubstationId = substation.Id;
                await _imageService.UpdateSubOrTmImageAsync(image);
            }
        }

        return substation;
    }
    public async Task<bool> DeleteSubstationAsync(int id)
    {
        var substation = await _substationRepository.GetByIdAsync(id);

        if (substation == null)
            throw new NotFoundException($"No districts found for region ID {id}.");

        await _substationRepository.DeleteAsync(substation.Id);
        return true;
    }
    public async Task ValidateRegionAndDistrictAsync(SubstationDto dto)
    {
        var region = await _regionRepository.GetByIdAsync(dto.RegionId);
        if (region == null)
            throw new Exception("Region not found!");

        var district = await _districtRepository.GetByIdAsync(dto.DistrictId);
        if (district == null || district.RegionId != dto.RegionId)
            throw new Exception("District not found or does not belong to the selected region");
    }
}