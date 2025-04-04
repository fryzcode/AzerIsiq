using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface ISubscriberService
{
    Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto);
    Task<Subscriber> CreateSubscriberCodeAsync(int id);
    Task<Subscriber> CreateCounterForSubscriberAsync(int id, CounterDto dto);
    Task<Subscriber> ConnectTmToSubscriberAsync(int id, int tmId);
    Task<(bool IsConfirmed, Subscriber Subscriber)> ApplySubscriberContractAsync(int id);
    Task<SubscriberDto> GetSubscriberByIdAsync(int id);
    Task<PagedResultDto<SubscriberDto>> GetSubscribersFilteredAsync(PagedRequestDto request,
        SubscriberFilterDto dtoFilter);
}