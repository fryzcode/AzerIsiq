using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IDistrictRepository : IReadOnlyRepository<District>
{
    Task<IEnumerable<Substation>> GetSubstationsByDistrictAsync(int districtId);
}