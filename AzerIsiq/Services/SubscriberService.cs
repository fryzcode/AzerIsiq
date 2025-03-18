using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IRegionRepository _regionRepository;


    public SubscriberService(ISubscriberRepository subscriberRepository, IRegionRepository regionRepository)
    {
        _subscriberRepository = subscriberRepository;
        _regionRepository = regionRepository;
    }
    
    public async Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto)
    {
        var atsCode = await _subscriberRepository.GenerateUniqueAtsAsync();
        
        var subscriber = new Subscriber()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Patronymic = dto.Patronymic,
            PhoneNumber = dto.PhoneNumber,
            FinCode = dto.FinCode,
            PopulationStatus = dto.PopulationStatus,
            RegionId = dto.RegionId,
            DistrictId = dto.DistrictId,
            Building = dto.Building.ToLower(),
            Apartment = dto.Apartment.ToLower(),
            Ats = atsCode
        };
        
        var result = await _subscriberRepository.CreateAsync(subscriber);

        return result;
    }
    public async Task<Subscriber> CreateSubscriberCodeAsync(int id)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id);
        if (subscriber == null)
        {
            throw new Exception("Not Found");
        }

        var districtId = subscriber.DistrictId.ToString().PadLeft(2, '0');
        var building = subscriber.Building.ToLower().PadLeft(4, '0');
        var apartment = subscriber.Apartment.ToLower().PadLeft(4, '0');

        var sbCode = $"{districtId}08000{building}{apartment}";
    
        Console.WriteLine(sbCode);
    
        subscriber.SubscriberCode = sbCode;
        subscriber.Status++;
    
        await _subscriberRepository.UpdateAsync(subscriber); 
    
        return subscriber;
    }

    // public async Task<Subscriber> CreateCounterForSubscriberAsync(int id)
    // {
    //     
    // }
    public async Task<PagedResultDto<GetSubscriberDto>> GetSubscribersAsync(int page, int pageSize)
    {
        var (subscribers, totalCount) = await _subscriberRepository.GetSubscribersAsync(page, pageSize);

        var subscriberDtos = subscribers.Select(s => new GetSubscriberDto
        {
            Id = s.Id,
            Name = s.Name,
            Surname = s.Surname,
            Patronymic = s.Patronymic,
            PhoneNumber = s.PhoneNumber,
            FinCode = s.FinCode,
            PopulationStatus = s.PopulationStatus,
            RegionId = s.RegionId,
            RegionName = s.Region?.Name ?? "N/A", 
            DistrictId = s.DistrictId,
            DistrictName = s.District?.Name ?? "N/A", 
            Building = s.Building,
            Apartment = s.Apartment,
            Status = s.Status,
            Ats = s.Ats,
            CreatedDate = s.CreatedAt.ToLocalTime()
        }).ToList();

        return new PagedResultDto<GetSubscriberDto>
        {
            Items = subscriberDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}