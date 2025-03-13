using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ISubscriberRepository _subscriberRepository;

    public SubscriberService(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;
    }
    
    public async Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto)
    {
        
        var subscriber = new Subscriber()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Patronymic = dto.Patronymic,
            PhoneNumber = dto.PhoneNumber,
            FinCode = dto.FinCode,
            PopulationStatus = dto.PopulationStatus,
            CityId = dto.CityId,
            DistrictId = dto.DistrictId,
            Building = dto.Building,
            Apartment = dto.Apartment
        };
        
        var result = await _subscriberRepository.CreateAsync(subscriber);

        return result;
    }
    
}