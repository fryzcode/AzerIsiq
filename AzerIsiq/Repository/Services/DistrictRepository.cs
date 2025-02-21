using AzerIsiq.Data;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Services;

public class DistrictRepository : ReadOnlyRepository<District>
{
    public DistrictRepository(AppDbContext context) : base(context) { }
}