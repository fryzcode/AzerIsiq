using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class DistrictService : ReadOnlyService<District>
{
    private readonly IDistrictRepository _districtRepository;
    public DistrictService(IReadOnlyRepository<District> repository, IDistrictRepository districtRepository) : base(repository)
    {
        _districtRepository = districtRepository;
    }
    public async Task<IEnumerable<SubstationDto>> GetSubstationsByDistrictAsync(int districtId)
    {
        var substations = await _districtRepository.GetSubstationsByDistrictAsync(districtId);
        
        var substationDtos = substations.Select(substation => new SubstationDto()
        {
            Id = substation.Id,
            Name = substation.Name,
            DistrictId = substation.DistrictId
        });
    
        return substationDtos;
    }
}