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
            Building = dto.Building,
            Apartment = dto.Apartment,
            Ats = atsCode
        };
        
        var result = await _subscriberRepository.CreateAsync(subscriber);

        return result;
    }
    
    public async Task<Subscriber> CreateSubscriberAtsAsync(SubscriberRequestDto dto)
    {
        
        var subscriber = new Subscriber()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Patronymic = dto.Patronymic,
            PhoneNumber = dto.PhoneNumber,
            FinCode = dto.FinCode,
            PopulationStatus = dto.PopulationStatus,
            // CityId = dto.CityId,
            DistrictId = dto.DistrictId,
            Building = dto.Building,
            Apartment = dto.Apartment
        };
        
        var result = await _subscriberRepository.CreateAsync(subscriber);

        return result;
    }

    // public async Task<GetSubscriberDto> GetSubscribersAsync()
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
    
    // public async Task<PagedResultDto<GetSubscriberDto>> GetSubscribersAsync(int page, int pageSize)
    // {
    //     var pagedSubscribers = await _subscriberRepository.GetPagedAsync(page, pageSize);
    //     var regionName = await _regionRepository.GetByIdAsync(dto.RegionId);
    //     var pagedSubscribers
    //     
    //     return new PagedResultDto<GetSubscriberDto>()
    //     {
    //         Items = pagedSubscribers.Items.Select(subscriber => new GetSubscriberDto()
    //         {
    //             Id = subscriber.Id,
    //             Ats = subscriber.Ats,
    //             RegionId = subscriber.RegionId,
    //             RegionName = _regionRepository.GetByIdAsync(subscriber.RegionId).Result.Name,;
    //             Name = subscriber.Name,
    //         }),
    //         TotalCount = pagedSubscribers.TotalCount,
    //         Page = page,
    //         PageSize = pageSize
    //     };
    // }
}