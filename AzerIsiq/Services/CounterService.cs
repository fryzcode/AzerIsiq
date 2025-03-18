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

    public async Task<Counter> CreateCountersAsync(CounterDto dto)
    {
        var counter = new Counter
        {
            Number = dto.Number,
            StampCode = dto.StampCode,
            Coefficient = dto.Coefficient,
            Volt = dto.Volt,
            Type = dto.Type,
        };
        
        await _counterRepository.CreateAsync(counter);
        return counter;
    }
}