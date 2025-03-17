using System.Linq.Expressions;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Extensions.Repository;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace AzerIsiq.Services;

public class SubstationService : ISubstationService
{
    private readonly ISubstationRepository _substationRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ILocationService _locationService;
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;
    private readonly LoggingService _loggingService;

    public SubstationService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository,
        IImageRepository imageRepository,
        ILocationService locationService,
        IImageService imageService, 
        LoggingService loggingService)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
        _imageRepository = imageRepository;
        _locationService = locationService;
        _imageService = imageService;
        _loggingService = loggingService;
    }
    public async Task<SubstationGetDto> GetSubstationByIdAsync(int id)
    {
        var substation = await _substationRepository.GetByIdWithIncludesAsync(id);
    
        if (substation == null)
        {
            throw new NotFoundException($"No substation found by ID {id}.");
        }

        return new SubstationGetDto
        {
            Id = substation.Id,
            Name = substation.Name,
            District = substation.District == null ? null : new DistrictDto
            {
                Id = substation.District.Id,
                Name = substation.District.Name,
                RegionId = substation.District.RegionId,
                Region = substation.District.Region == null ? null : new RegionDto
                {
                    Id = substation.District.Region.Id,
                    Name = substation.District.Region.Name
                }
            },
            Location = substation.Location == null ? null : new LocationDto
            {
                Id = substation.Location.Id,
                Latitude = substation.Location.Latitude,
                Longitude = substation.Location.Longitude,
                Address = substation.Location.Address
            },
            Images = substation.Images?.Select(img => new ImageDto
            {
                Id = img.Id,
                ImageName = img.ImageName
            }).ToList()
        };
    }
    public async Task<PagedResultDto<SubstationResponseDto>> GetSubstationAsync(int page, int pageSize)
    {
        var pagedSubs = await _substationRepository.GetPagedAsync(page, pageSize);
        
        return new PagedResultDto<SubstationResponseDto>()
        {
            Items = pagedSubs.Items.Select(sub => new SubstationResponseDto()
            {
                Id = sub.Id,
                Name = sub.Name,
                DistrictId = sub.DistrictId,
            }),
            TotalCount = pagedSubs.TotalCount,
            Page = page,
            PageSize = pageSize
        };
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

        await _loggingService.LogActionAsync("Create", nameof(Subscriber), createdSubstation.Id);
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
        
        await _loggingService.LogActionAsync("Edit", nameof(Subscriber), id);

        return substation;
    }
    public async Task<bool> DeleteSubstationAsync(int id)
    {
        var substation = await _substationRepository.GetByIdAsync(id);
        
        if (substation == null)
            throw new NotFoundException($"No substation found by ID {id}.");
        
        if (substation.LocationId.HasValue)
        {
            await _locationService.DeleteLocationAsync(substation.LocationId.Value);
        }

        if (substation.Images.Any())
        {
            foreach (var image in substation.Images)
            {
                await _imageService.DeleteImageAsync(image.Id);
            }
        }
        
        await _substationRepository.DeleteAsync(substation.Id);
        
        await _loggingService.LogActionAsync("Delete", nameof(Subscriber), id);
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
    
    public async Task<PagedResultDto<SubstationDto>> GetSubstationByDistrictAsync(PagedRequestDto request, int districtId)
    {
        Expression<Func<Substation, bool>> filter = sb => sb.DistrictId == districtId;

        if (!string.IsNullOrEmpty(request.Search))
        {
            filter = sb => sb.DistrictId == districtId && sb.Name.Contains(request.Search);
        }

        var pagedSubstations = await _substationRepository.GetPageAsync(request.Page, request.PageSize, filter);

        return new PagedResultDto<SubstationDto>
        {
            Items = pagedSubstations.Items.Select(sb => new SubstationDto
            {
                Id = sb.Id,
                Name = sb.Name,
                DistrictId = sb.DistrictId
            }).ToList(),
            TotalCount = pagedSubstations.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

}