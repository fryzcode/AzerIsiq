using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class ElectronicAppealRepository : GenericRepository<ElectronicAppeal>, IElectronicAppealRepository
{
    public ElectronicAppealRepository(AppDbContext context) : base(context)
    {
    }
    
}