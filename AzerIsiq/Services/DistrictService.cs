using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class DistrictService : ReadOnlyService<District>
{
    public DistrictService(IReadOnlyRepository<District> repository) : base(repository) { }
}