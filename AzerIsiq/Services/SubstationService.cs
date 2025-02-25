using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class SubstationService
{
    private readonly ISubstationRepository _substationRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;

    public SubstationService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
    }

    public async Task<Substation> CreateSubstationAsync(SubstationDto dto)
    {
        var region = await _regionRepository.GetByIdAsync(dto.RegionId);
        if (region == null)
            throw new Exception("Region not found!");

        var district = await _districtRepository.GetByIdAsync(dto.DistrictId);
        if (district == null || district.RegionId != dto.RegionId)
            throw new Exception("District not found or does not belong to the selected region");

        var substation = new Substation
        {
            Name = dto.Name,
            DistrictId = dto.DistrictId
        };

        return await _substationRepository.CreateAsync(substation);
    }
}