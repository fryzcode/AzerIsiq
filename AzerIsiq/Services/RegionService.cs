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