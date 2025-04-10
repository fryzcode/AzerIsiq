using System.Linq.Expressions;
using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Extensions.Repository;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class SubstationService : ISubstationService
{
    private readonly ISubstationRepository _substationRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ILocationService _locationService;
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;
    private readonly ILoggingService _loggingService;
    private readonly IMapper _mapper;

    public SubstationService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository,
        IImageRepository imageRepository,
        ILocationService locationService,
        IImageService imageService, 
        ILoggingService loggingService,
        IMapper mapper)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
        _imageRepository = imageRepository;
        _locationService = locationService;
        _imageService = imageService;
        _loggingService = loggingService;
        _mapper = mapper;
    }
    public async Task<SubstationGetDto> GetSubstationByIdAsync(int id)
    {
        var substation = await _substationRepository.GetByIdWithIncludesAsync(id)
                         ?? throw new NotFoundException($"No substation found by ID {id}.");

        return _mapper.Map<SubstationGetDto>(substation);
    }
    public async Task<PagedResultDto<SubstationResponseDto>> GetSubstationAsync(int page, int pageSize)
    {
        var pagedSubs = await _substationRepository.GetPagedAsync(page, pageSize);

        return new PagedResultDto<SubstationResponseDto>()
        {
            Items = pagedSubs.Items.Select(_mapper.Map<SubstationResponseDto>),
            TotalCount = pagedSubs.TotalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<Substation> CreateSubstationAsync(SubstationDto dto)
    {
        await ValidateRegionAndDistrictAsync(dto);

        Location? location = null;

        if (!string.IsNullOrWhiteSpace(dto.Latitude) && !string.IsNullOrWhiteSpace(dto.Longitude))
        {
            location = await _locationService.CreateLocationAsync(dto.Latitude, dto.Longitude, dto.Address);
        }
        
        var substation = _mapper.Map<Substation>(dto);
        substation.LocationId = location?.Id;
    
        var createdSubstation = await _substationRepository.CreateAsync(substation);

        if (dto.Image != null)
        {
            var image = await _imageService.UploadImageAsync(dto.Image);
            image.SubstationId = createdSubstation.Id;

            await _imageService.UpdateSubOrTmImageAsync(image);
        }

        await _loggingService.LogActionAsync("Create", nameof(Substation), createdSubstation.Id);
        return createdSubstation;
    }
    public async Task<Substation> EditSubstationAsync(int id, SubstationDto dto)
    {
        var substation = await _substationRepository.GetByIdAsync(id)
                         ?? throw new NotFoundException($"No substation found with ID {id}.");

        if (dto.RegionId > 0 && dto.DistrictId > 0)
        {
            await ValidateRegionAndDistrictAsync(dto);
        }

        if (!string.IsNullOrWhiteSpace(dto.Latitude) && !string.IsNullOrWhiteSpace(dto.Longitude))
        {
            var location = await _locationService.CreateLocationAsync(dto.Latitude, dto.Longitude, dto.Address);
            substation.LocationId = location.Id;
        }

        _mapper.Map(dto, substation);

        await _substationRepository.UpdateAsync(substation);

        if (dto.Image != null)
        {
            await UpdateSubstationImageAsync(substation.Id, dto.Image);
        }

        await _loggingService.LogActionAsync("Edit", nameof(Substation), id);

        return substation;
    }
    public async Task<bool> DeleteSubstationAsync(int id)
    {
        var substation = await _substationRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException($"No substation found by ID {id}.");
        
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
        
        await _loggingService.LogActionAsync("Delete", nameof(Substation), id);
        return true;
    }
    public async Task ValidateRegionAndDistrictAsync(SubstationDto dto)
    {
        var region = await _regionRepository.GetByIdAsync(dto.RegionId)
                     ?? throw new NotFoundException("Region not found!");

        var district = await _districtRepository.GetByIdAsync(dto.DistrictId);
        if (district == null || district.RegionId != dto.RegionId)
                    throw new NotFoundException("District not found or does not belong to the selected region");
    }
    public async Task<PagedResultDto<SubstationDto>> GetSubstationByDistrictAsync(PagedRequestDto request, int districtId)
    {
        Expression<Func<Substation, bool>> filter = sb => sb.DistrictId == districtId;

        if (!string.IsNullOrEmpty(request.Search))
        {
            filter = sb => sb.DistrictId == districtId && sb.Name.Contains(request.Search);
        }

        var pagedSubstations = await _substationRepository.GetPageAsync(request.Page, request.PageSize, filter);

        var mappedItems = _mapper.Map<List<SubstationDto>>(pagedSubstations.Items);

        return new PagedResultDto<SubstationDto>
        {
            Items = mappedItems,
            TotalCount = pagedSubstations.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
    public async Task<PagedResultDto<SubstationDto>> GetSubstationsByFiltersAsync(PagedRequestDto request, int? regionId, int? districtId)
    {
        var pagedSubstations = await _substationRepository.GetSubstationsByFiltersAsync(regionId, districtId, request.Page, request.PageSize);

        return new PagedResultDto<SubstationDto>
        {
            Items = pagedSubstations.Items.Select(_mapper.Map<SubstationDto>).ToList(),
            TotalCount = pagedSubstations.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
    private async Task UpdateSubstationImageAsync(int substationId, IFormFile imageFile)
    {
        var existingImage = await _imageService.GetImageBySubstationIdAsync(substationId);
    
        if (existingImage != null)
        {
            var updateDto = new ImageUpdateDto
            {
                Id = existingImage.Id,
                File = imageFile,
                SubstationId = substationId
            };
            await _imageService.UpdateImageAsync(updateDto);
        }
        else
        {
            var image = await _imageService.UploadImageAsync(imageFile);
            image.SubstationId = substationId;
            await _imageService.UpdateSubOrTmImageAsync(image);
        }
    }
}