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
    private readonly IAuthService _authService;
    private readonly ICounterRepository _counterRepository;
    
    public SubscriberService(
        ISubscriberRepository subscriberRepository, 
        ICounterService counterService, 
        ITmService tmService, 
        IMapper mapper, 
        ISubscriberCodeGenerator codeGenerator,
        ILoggingService loggingService, 
        IAuthService authService,
        ICounterRepository counterRepository
        )
    {
        _subscriberRepository = subscriberRepository;
        _counterService = counterService;
        _tmService = tmService;
        _mapper = mapper;
        _codeGenerator = codeGenerator;
        _loggingService = loggingService;
        _authService = authService;
        _counterRepository = counterRepository;
    }
    
    public async Task<Subscriber> CreateSubscriberRequestAsync(SubscriberRequestDto dto)
    {
        var userId = _authService.GetCurrentUserId();
        var requests = await _subscriberRepository.GetRequestsByFinAsync(dto.FinCode);
        
        if (requests.Count >= 3)
        {
            var firstRequest = requests.OrderBy(r => r.CreatedAt).FirstOrDefault();
        
            if (firstRequest != null && (DateTime.UtcNow - firstRequest.CreatedAt).TotalDays < 30)
            {
                throw new Exception($"Requests with FIN {dto.FinCode} exceed the limit");
            }
        }
        
        var userRequests = await _subscriberRepository.GetUserRequestsInLastMonthAsync(userId);

        if (userRequests.Count >= 3)
        {
            throw new Exception("You have reached the request limit (3 per month).");
        }
        
        var atsCode = await _subscriberRepository.GenerateUniqueAtsAsync();
        
        var subscriber = _mapper.Map<Subscriber>(dto);
        subscriber.Building = dto.Building.ToLower();
        subscriber.Apartment = dto.Apartment.ToLower();
        subscriber.Ats = atsCode;
        subscriber.UserId = _authService.GetCurrentUserId();
        
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

        var counter = await _counterService.CreateCountersAsync(dto, id);
        
        subscriber.Status = SubscriberStatusHelper.AdvanceStatus(subscriber.Status, SubscriberStatus.CodeGenerated);
        
        await _subscriberRepository.UpdateAsync(subscriber); 
        await _loggingService.LogActionAsync("Create Counter and Connect", nameof(Subscriber), subscriber.Id);
        return subscriber;
    }
    public async Task<Subscriber> UpdateCounterForSubscriberAsync(int id, CounterDto dto)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(id)
                         ?? throw new NotFoundException("Subscriber not found");

        var oldCounters = await _counterRepository.GetBySubscriberIdAsync(subscriber.Id);

        if (oldCounters == null || !oldCounters.Any())
            throw new NotFoundException("This subscriber does not have any counters");

        foreach (var oldCounter in oldCounters)
        {
            oldCounter.Status = 2;
            await _counterRepository.UpdateAsync(oldCounter);
        }
        
        var counter = await _counterService.CreateCountersAsync(dto, subscriber.Id);
        
        await _loggingService.LogActionAsync("Update Counter", nameof(Counter), counter.Id);
        await _loggingService.LogActionAsync("Connected Counter to Subscriber", nameof(Subscriber), subscriber.Id);
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
    public async Task<List<SubscriberProfileDto>> GetProfilesAsync(int userId)
    {
        var subscribers = await _subscriberRepository.GetByUserIdAsync(userId);
        if (subscribers == null || !subscribers.Any())
        {
            throw new NotFoundException("Abunə tapılmadı");
        }

        return _mapper.Map<List<SubscriberProfileDto>>(subscribers);
    }
    public async Task<SubscriberDebtDto> GetDebtBySubscriberCodeAsync(string subscriberCode)
    {
        var subscriber = await _subscriberRepository.GetWithCountersByCodeAsync(subscriberCode)
                         ?? throw new NotFoundException("Subscriber not found");

        return _mapper.Map<SubscriberDebtDto>(subscriber);
    }
}