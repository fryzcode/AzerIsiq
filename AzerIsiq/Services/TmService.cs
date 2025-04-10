using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using System;
using System.Threading.Tasks;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Extensions.Repository;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class TmService : ITmService
{
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ISubstationRepository _substationRepository;
    private readonly ITmRepository _tmRepository;
    private readonly ILocationService _locationService;
    private readonly ILoggingService _loggingService;

    public TmService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository,
        ITmRepository tmRepository,
        ILocationService locationService,
        ILoggingService loggingService)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
        _tmRepository = tmRepository;
        _locationService = locationService;
        _loggingService = loggingService;
    }

    public async Task<Tm> GetTmByIdAsync(int id)
    {
        var tm = await _tmRepository.GetByIdAsync(id)
                   ?? throw new NotFoundException($"No tm found by ID {id}.");
        
        return tm;
    }
    public async Task<PagedResultDto<TmDtoPaged>> GetTmsByFiltersAsync(PagedRequestDto request, int? regionId, int? districtId, int? substationId)
    {
        var pagedTms = await _tmRepository.GetTmsByFiltersAsync(regionId, districtId, substationId, request.Page, request.PageSize);

        return new PagedResultDto<TmDtoPaged>
        {
            Items = pagedTms.Items.Select(t => new TmDtoPaged
            {
                Id = t.Id,
                Name = t.Name,
            }).ToList(),
            TotalCount = pagedTms.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
    public async Task<PagedResultDto<TmResponeDto>> GetTmAsync(int page, int pageSize)
    {
        var pagedTms = await _tmRepository.GetPagedAsync(page, pageSize);
        
        return new PagedResultDto<TmResponeDto>()
        {
            Items = pagedTms.Items.Select(tm => new TmResponeDto()
            {
                Id = tm.Id,
                Name = tm.Name,
                SubstationId = tm.SubstationId,
            }),
            TotalCount = pagedTms.TotalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<Tm> CreateTmAsync(TmDto dto)
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
        
        await ValidateTmDataAsync(dto);

        var tm = new Tm
        {
            Name = dto.Name,
            SubstationId = dto.SubstationId,
            LocationId = location?.Id
        };
        
        await _tmRepository.CreateAsync(tm);
        
        await _loggingService.LogActionAsync("Create", nameof(Tm), tm.Id);
        return tm;
    }
    public async Task<Tm> EditTmAsync(int id, TmDto dto)
    {
        var tm = await _tmRepository.GetByIdAsync(id)
                 ?? throw new NotFoundException($"No transformator found for ID {id}.");
        
        if (dto.RegionId > 0 && dto.DistrictId > 0 && dto.SubstationId > 0)
        {
            await ValidateTmDataAsync(dto);
        }

        if (!string.IsNullOrEmpty(dto.Longitude) && !string.IsNullOrEmpty(dto.Latitude))
        {
            var location = await _locationService.CreateLocationAsync(dto.Latitude, dto.Longitude, dto.Address);
            tm.LocationId = location.Id;
        }
        
        if (!string.IsNullOrEmpty(dto.Name))
            tm.Name = dto.Name;

        if (dto.SubstationId > 0)
            tm.SubstationId = dto.SubstationId;
        
        await _tmRepository.UpdateAsync(tm);
        await _loggingService.LogActionAsync("Edit", nameof(Tm), id);
        return tm;
    }
    public async Task<bool> DeleteTmAsync(int id)
    {
        var tm = await _tmRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException($"No tm found by ID {id}.");
        
        if (tm.LocationId.HasValue)
        {
            await _locationService.DeleteLocationAsync(tm.LocationId.Value);
        }
        
        await _substationRepository.DeleteAsync(tm.Id);
        await _loggingService.LogActionAsync("Delete", nameof(Tm), id);
        return true;
    }
    public async Task ValidateTmDataAsync(TmDto dto)
    {
        var region = await _regionRepository.GetByIdAsync(dto.RegionId)
                     ?? throw new Exception("Region not found!");

        var district = await _districtRepository.GetByIdAsync(dto.DistrictId);
        if (district == null || district.RegionId != dto.RegionId)
            throw new Exception("District not found or does not belong to the selected region");

        var substation = await _substationRepository.GetByIdAsync(dto.SubstationId);
        if (substation == null || substation.DistrictId != dto.DistrictId)
            throw new Exception("SubstationDto not found or does not belong to the selected district");
    }
}
