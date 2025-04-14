using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Enum;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.Helpers;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly ICounterService _counterService;
    private readonly ITmService _tmService;
    private readonly IMapper _mapper;
    private readonly ISubscriberCodeGenerator _codeGenerator;
    private readonly ILoggingService _loggingService;
    
    public SubscriberService(
        ISubscriberRepository subscriberRepository, 
        ICounterService counterService, 
        ITmService tmService, 
        IMapper mapper, 
        ISubscriberCodeGenerator codeGenerator,
        ILoggingService loggingService
        )
    {
        _subscriberRepository = subscriberRepository;
        _counterService = counterService;
        _tmService = tmService;
        _mapper = mapper;
        _codeGenerator = codeGenerator;
        _loggingService = loggingService;
    }
    
    public async Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto)
    {
        var checkFin = await _subscriberRepository.ExistsBySubscriberFinAsync(dto.FinCode);
        
        if (checkFin)
        {
            throw new Exception($"SubscriberCode {dto.FinCode} already exists.");
        }
        
        var atsCode = await _subscriberRepository.GenerateUniqueAtsAsync();
        
        var subscriber = _mapper.Map<Subscriber>(dto);
        subscriber.Building = dto.Building.ToLower();
        subscriber.Apartment = dto.Apartment.ToLower();
        subscriber.Ats = atsCode;
        
        var result = await _subscriberRepository.CreateAsync(subscriber);

        return result;
    }
    public async Task<Subscriber> CreateSubscriberCodeAsync(int id)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id)
                         ?? throw new NotFoundException("Subscriber not found");

        var sbCode = _codeGenerator.Generate(subscriber);

        if (await _subscriberRepository.ExistsBySubscriberCodeAsync(sbCode))
        {
            throw new Exception($"SubscriberCode {sbCode} already exists.");
        }

        subscriber.SubscriberCode = sbCode;
        
        subscriber.Status = SubscriberStatusHelper.AdvanceStatus(subscriber.Status, SubscriberStatus.Initial);

        await _subscriberRepository.UpdateAsync(subscriber);
        await _loggingService.LogActionAsync("Create Subscriber Code", nameof(Subscriber), subscriber.Id);
        return subscriber;
    }
    public async Task<Subscriber> CreateCounterForSubscriberAsync(int id, CounterDto dto)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id)
                         ?? throw new NotFoundException("Subscriber not found");

        var counter = await _counterService.CreateCountersAsync(dto);
        
        subscriber.CounterId = counter.Id;
        
        subscriber.Status = SubscriberStatusHelper.AdvanceStatus(subscriber.Status, SubscriberStatus.CodeGenerated);
        
        await _subscriberRepository.UpdateAsync(subscriber); 
        await _loggingService.LogActionAsync("Create Counter and Connect", nameof(Subscriber), subscriber.Id);
        return subscriber;
    }
    public async Task<Subscriber> ConnectTmToSubscriberAsync(int id, int tmId)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id)
                         ?? throw new NotFoundException("Subscriber not found");

        await _tmService.GetTmByIdAsync(tmId);
        
        subscriber.TmId = tmId;
        
        subscriber.Status = SubscriberStatusHelper.AdvanceStatus(subscriber.Status, SubscriberStatus.CounterConnected);
        
        await _subscriberRepository.UpdateAsync(subscriber);
        await _loggingService.LogActionAsync("Connect Transformator", nameof(Subscriber), subscriber.Id);
        return subscriber;
    }
    public async Task<(bool IsConfirmed, Subscriber Subscriber)> ApplySubscriberContractAsync(int id)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id)
                         ?? throw new NotFoundException("Subscriber not found");
    
        if (subscriber.Status >= (int)SubscriberStatus.ContractSigned)
        {
            return (true, subscriber);
        }

        if (subscriber.Status == (int)SubscriberStatus.TmConnected)
        {
            subscriber.Status = (int)SubscriberStatus.ContractSigned;
            await _subscriberRepository.UpdateAsync(subscriber);
        }
        
        await _loggingService.LogActionAsync("Apply Contract", nameof(Subscriber), subscriber.Id);
        return (false, subscriber);
    }
    public async Task<PagedResultDto<SubscriberDto>> GetSubscribersFilteredAsync(PagedRequestDto request, SubscriberFilterDto dtoFilter)
    {
        var subscribers = await _subscriberRepository.GetSubscriberByFiltersAsync(dtoFilter);
        var subscriberDtos = _mapper.Map<List<SubscriberDto>>(subscribers.Items);
        return new PagedResultDto<SubscriberDto>
        {
            Items = subscriberDtos,
            TotalCount = subscribers.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
    public async Task<SubscriberDto> GetSubscriberByIdAsync(int id)
    {
        var sb = await _subscriberRepository.GetByIdAsync(id, 
            s => s.Region, 
            s => s.District, 
            s => s.Territory, 
            s => s.Street);
            
        if (sb == null)
        {
            throw new NotFoundException($"Not found Subscriber by ID {id}.");
        }

        return _mapper.Map<SubscriberDto>(sb);
    }

}