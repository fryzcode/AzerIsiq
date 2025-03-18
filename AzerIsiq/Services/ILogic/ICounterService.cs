using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface ICounterService
{
    Task<Counter> CreateCountersAsync(CounterDto dto);
}