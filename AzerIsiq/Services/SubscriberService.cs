using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ICounterService _counterService;
    private readonly ITmService _tmService;
    
    public SubscriberService(ISubscriberRepository subscriberRepository, IRegionRepository regionRepository, ICounterService counterService, ITmService tmService)
    {
        _subscriberRepository = subscriberRepository;
        _regionRepository = regionRepository;
        _counterService = counterService;
        _tmService = tmService;
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
            TerritoryId = dto.TerritoryId,
            StreetId = dto.StreetId,
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
        var territoryId = (subscriber.TerritoryId?.ToString() ?? "00").PadLeft(2, '0');
        var streetId = (subscriber.StreetId?.ToString() ?? "000").PadLeft(3, '0');
        var building = (subscriber.Building ?? "0").PadLeft(4, '0');
        var apartment = (subscriber.Apartment ?? "0").PadLeft(4, '0');

        var sbCode = $"{districtId}{territoryId}{streetId}{building}{apartment}";
    
        Console.WriteLine(sbCode);
    
        subscriber.SubscriberCode = sbCode;

        if (subscriber.Status == 1)
        {
            subscriber.Status++;
        }
        
        await _subscriberRepository.UpdateAsync(subscriber); 
    
        return subscriber;
    }
    public async Task<Subscriber> CreateCounterForSubscriberAsync(int id, CounterDto dto)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id);
        if (subscriber == null)
        {
            throw new Exception("Not Found");
        }

        var counter = await _counterService.CreateCountersAsync(dto);
        
        subscriber.CounterId = counter.Id;
        
        if (subscriber.Status == 2)
        {
            subscriber.Status++;
        }
        
        await _subscriberRepository.UpdateAsync(subscriber); 
        
        return subscriber;
    }
    public async Task<Subscriber> ConnectTmToSubscriberAsync(int id, int tmId)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id);
        if (subscriber == null)
        {
            throw new Exception("Not Found");
        }

        await _tmService.GetTmByIdAsync(tmId);
        
        subscriber.TmId = tmId;
        
        if (subscriber.Status == 3)
        {
            subscriber.Status++;
        }
        
        await _subscriberRepository.UpdateAsync(subscriber); 
        return subscriber;
    }
    public async Task<PagedResultDto<SubscriberDto>> GetSubscribersFilteredAsync(PagedRequestDto request, SubscriberFilterDto dtoFilter)
    {
        var subscribers = await _subscriberRepository.GetSubscriberByFiltersAsync(dtoFilter);

        return new PagedResultDto<SubscriberDto>
        {
            Items = subscribers.Items.Select(s => new SubscriberDto
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname,
                Patronymic = s.Patronymic,
                PhoneNumber = s.PhoneNumber,
                FinCode = s.FinCode,
                PopulationStatus = s.PopulationStatus,
                RegionId = s.RegionId,
                RegionName = s.RegionName,
                DistrictId = s.DistrictId,
                DistrictName = s.DistrictName,
                TerritoryId = s.TerritoryId,
                TerritoryName = s.TerritoryName,
                StreetId = s.StreetId,
                StreetName = s.StreetName,
                Building = s.Building,
                Apartment = s.Apartment,
                Status = s.Status,
                Ats = s.Ats,
                SubscriberCode = s.SubscriberCode,
                CreatedAt = s.CreatedAt
            }).ToList(),
            TotalCount = subscribers.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
    
    public async Task<SubscriberDto> GetSubscriberByIdAsync(int id)
    {
        var sb = await _subscriberRepository.GetByIdAsync(id);
    
        if (sb == null)
        {
            throw new NotFoundException($"Not found Subscriber by ID {id}.");
        }
    
        return new SubscriberDto
        {
            Id = sb.Id,
            Name = sb.Name,
            Surname = sb.Surname,
            Patronymic = sb.Patronymic,
            PhoneNumber = sb.PhoneNumber,
            FinCode = sb.FinCode,
            RegionId = sb.RegionId,
            RegionName = sb.Region?.Name ?? "N/A",
            DistrictId = sb.DistrictId,
            DistrictName = sb.District?.Name ?? "N/A",
            TerritoryId = sb.TerritoryId,
            TerritoryName = sb.Territory?.Name ?? "N/A",
            StreetId = sb.StreetId,
            StreetName = sb.Street?.Name ?? "N/A",
            Building = sb.Building,
            Apartment = sb.Apartment,
            Status = sb.Status,
            Ats = sb.Ats,
            SubscriberCode = sb.SubscriberCode,
            CreatedAt = sb.CreatedAt
        };
    }
}