using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class DistrictService : ReadOnlyService<District>, IDistrictService
{
    private readonly IDistrictRepository _districtRepository;

    public DistrictService(IReadOnlyRepository<District> repository, IDistrictRepository districtRepository)
        : base(repository)
    {
        _districtRepository = districtRepository ?? throw new ArgumentNullException(nameof(districtRepository));
    }

    public async Task<IEnumerable<SubstationDto>> GetSubstationsByDistrictAsync(int districtId)
    {
        ValidateDistrictId(districtId);

        var substations = await _districtRepository.GetSubstationsByDistrictAsync(districtId);
        return substations.Select(substation => new SubstationDto
        {
            Id = substation.Id,
            Name = substation.Name,
            DistrictId = substation.DistrictId
        });
    }
    
    public async Task<IEnumerable<TerritoryDto>> GetTerritoryByDistrictAsync(int districtId)
    {
        ValidateDistrictId(districtId);

        var territories = await _districtRepository.GetTerritoryByDistrictAsync(districtId);
        return territories.Select(territory => new TerritoryDto()
        {
            Id = territory.Id,
            Name = territory.Name,
            DistrictId = territory.DistrictId
        });
    }
    
    public async Task<IEnumerable<StreetDto>> GetStreetByTerritoryAsync(int territiryId)
    {
        ValidateDistrictId(territiryId);

        var streets = await _districtRepository.GetStreetByTerritoryAsync(territiryId);
        return streets.Select(street => new StreetDto()
        {
            Id = street.Id,
            Name = street.Name,
            TerritoryId = street.TerritoryId
        });
    }

    public async Task<IEnumerable<TmDto>> GetTmsByDistrictAsync(int districtId)
    {
        ValidateDistrictId(districtId);

        var tms = await _districtRepository.GetTmsByDistrictAsync(districtId);
        if (!tms.Any()) throw new NotFoundException($"No TMs found for district ID {districtId}.");

        return tms.Select(tm => new TmDto
        {
            Id = tm.Id,
            Name = tm.Name
        });
    }

    private static void ValidateDistrictId(int id)
    {
        if (id <= 0) throw new ArgumentException("ID must be greater than zero.", nameof(id));
    }
    
}