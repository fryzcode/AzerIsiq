using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface ISubscriberService
{
    Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto);
    Task<PagedResultDto<GetSubscriberDto>> GetSubscribersAsync(int page, int pageSize);
}