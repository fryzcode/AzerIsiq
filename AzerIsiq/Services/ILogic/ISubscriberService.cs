using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface ISubscriberService
{
    Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto);
    Task<Subscriber> CreateSubscriberCodeAsync(int id);
    // Task<Subscriber> CreateCounterForSubscriberAsync(int id);
    Task<PagedResultDto<GetSubscriberDto>> GetSubscribersAsync(int page, int pageSize);
}