using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface ISubstationService
{
    Task<Substation> CreateSubstationAsync(SubstationDto dto);
    Task<Substation> EditSubstationAsync(int id, SubstationDto dto);
    Task<bool> DeleteSubstationAsync(int id);
    Task ValidateRegionAndDistrictAsync(SubstationDto dto);
}