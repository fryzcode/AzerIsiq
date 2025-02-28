using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace AzerIsiq.Services;

public class TmService
{
    private readonly IRegionRepository _regionRepository;
    private readonly IDistrictRepository _districtRepository;
    private readonly ISubstationRepository _substationRepository;
    private readonly ITmRepository _tmRepository;

    public TmService(
        ISubstationRepository substationRepository,
        IRegionRepository regionRepository,
        IDistrictRepository districtRepository,
        ITmRepository tmRepository)
    {
        _substationRepository = substationRepository;
        _regionRepository = regionRepository;
        _districtRepository = districtRepository;
        _tmRepository = tmRepository;
    }

    public async Task<Tm> CreateTmAsync(TmDto dto)
    {
        await ValidateTmDataAsync(dto);

        var tm = new Tm
        {
            Name = dto.Name,
            SubstationId = dto.SubstationId
        };

        await _tmRepository.CreateAsync(tm);
        
        return tm;
    }

    public async Task<Tm> EditTmAsync(int id, TmDto dto)
    {
        await ValidateTmDataAsync(dto);

        var tm = await _tmRepository.GetByIdAsync(id);
        if (tm == null)
            throw new Exception("TM not found!");

        tm.Name = dto.Name;
        tm.SubstationId = dto.SubstationId;

        await _tmRepository.UpdateAsync(tm);
        
        return tm;
    }

    private async Task ValidateTmDataAsync(TmDto dto)
    {
        var region = await _regionRepository.GetByIdAsync(dto.RegionId)
                     ?? throw new Exception("Region not found!");

        var district = await _districtRepository.GetByIdAsync(dto.DistrictId);
        if (district == null || district.RegionId != dto.RegionId)
            throw new Exception("District not found or does not belong to the selected region");

        var substation = await _substationRepository.GetByIdAsync(dto.SubstationId);
        if (substation == null || substation.DistrictId != dto.DistrictId)
            throw new Exception("Substation not found or does not belong to the selected district");
    }
}
