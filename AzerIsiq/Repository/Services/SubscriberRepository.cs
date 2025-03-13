using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Repository.Services;

public class SubscriberRepository : GenericRepository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(AppDbContext context) : base(context)
    {
        
    }
}