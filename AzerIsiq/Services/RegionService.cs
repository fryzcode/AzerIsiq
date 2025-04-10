using System.Linq.Expressions;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class RegionService : ReadOnlyService<Region>, IRegionService
{
    private readonly IRegionRepository _regionRepository;

    public RegionService(IReadOnlyRepository<Region> repository, IRegionRepository regionRepository) : base(repository)
    {
        _regionRepository = regionRepository;
    }
    
    public async Task<PagedResultDto<RegionDto>> GetRegionAsync(int page, int pageSize)
    {
        var pagedRegions = await _regionRepository.GetPagedAsync(page, pageSize);
        
        return new PagedResultDto<RegionDto>()
        {
            Items = pagedRegions.Items.Select(region => new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
            }),
            TotalCount = pagedRegions.TotalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    public async Task<IEnumerable<DistrictDto>> GetDistrictsByRegionAsync(int regionId)
    {
        var districts = await _regionRepository.GetDistrictsByRegionAsync(regionId);
        
        if (districts == null || !districts.Any())
        {
            throw new NotFoundException($"No districts found for region ID {regionId}.");
        }
        
        var districtDtos = districts.Select(district => new DistrictDto
        {
            Id = district.Id,
            Name = district.Name,
            RegionId = district.RegionId,
        });

        return districtDtos;
    }
    public async Task<IEnumerable<SubstationDto>> GetSubstationByDistrictAsync(int districtId)
    {
        var substations = await _regionRepository.GetSubstationsByDistrictAsync(districtId);
        
        if (substations == null || !substations.Any())
        {
            throw new NotFoundException($"No districts found for region ID {districtId}.");
        }

        var substationDtos = substations.Select(substation => new SubstationDto
        {
            Id = substation.Id,
            Name = substation.Name,
        });

        return substationDtos;
    }
    public async Task<IEnumerable<TmDto>> GetTmsBySubstationAsync(int substationId)
    {
        var tms = await _regionRepository.GetTmsBySubstationAsync(substationId);
        
        if (tms == null || !tms.Any())
        {
            throw new NotFoundException($"No districts found for region ID {substationId}.");
        }

        var tmDtos = tms.Select(tm => new TmDto()
        {
            Id = tm.Id,
            Name = tm.Name,
        });

        return tmDtos;
    }
    public async Task<IEnumerable<SubstationDto>> GetSubstationsByRegionAsync(int regionId)
    {
        var substations = await _regionRepository.GetSubstationsByRegionAsync(regionId);
        
        var substationDtos = substations.Select(Substation => new SubstationDto
        {
            // Id = SubstationDto.Id,
            Name = Substation.Name,
            DistrictId = Substation.DistrictId,
        });
        
        return substationDtos;
    }
    public async Task<IEnumerable<TmDto>> GetTmsByRegionAsync(int regionId)
    {
        var tms = await _regionRepository.GetTmsByRegionAsync(regionId);
    
        if (tms == null || !tms.Any())
        {
            throw new NotFoundException($"No TMs found for region ID {regionId}.");
        }

        var tmDtos = tms.Select(tm => new TmDto
        {
            Id = tm.Id,
            Name = tm.Name
        });

        return tmDtos;
    }

}