using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

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
            // Id = Substation.Id,
            Name = Substation.Name,
            DistrictId = Substation.DistrictId,
        });
        
        return substationDtos;
    }
}