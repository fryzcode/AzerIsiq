using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class CounterService : ICounterService
{
    private readonly ICounterRepository _counterRepository;

    public CounterService(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }

    public async Task<Counter> CreateCountersAsync(CounterDto dto, int subscriberId)
    {
        var counters = await _counterRepository.GetBySubscriberIdAsync(subscriberId);
        if (counters.Count >= 3)
        {
            throw new InvalidOperationException("This subscriber already has 3 counters.");
        }

        var counter = new Counter
        {
            Number = dto.Number,
            StampCode = dto.StampCode,
            Coefficient = dto.Coefficient,
            Phase = dto.Phase,
            Volt = dto.Volt,
            Type = dto.Type,
            SubscriberId = subscriberId
        };

        await _counterRepository.CreateAsync(counter);
        return counter;
    }

    
}