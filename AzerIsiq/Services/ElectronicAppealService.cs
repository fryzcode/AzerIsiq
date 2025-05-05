using AutoMapper;
using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class ElectronicAppealService : IElectronicAppealService
{
    private readonly IElectronicAppealRepository _repository;
    private readonly IMapper _mapper;
    
    public ElectronicAppealService(IElectronicAppealRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ElectronicAppealDto>> GetAllAsync()
    {
        var appeals = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ElectronicAppealDto>>(appeals);
    }

    public async Task<ElectronicAppealDto> GetByIdAsync(int id)
    {
        var appeal = await _repository.GetByIdAsync(id);
        if (appeal == null) throw new Exception("Not found");
        return _mapper.Map<ElectronicAppealDto>(appeal);
    }
    
    public async Task<ElectronicAppealDto> CreateAsync(ElectronicAppealCreateDto dto)
    {
        var entity = _mapper.Map<ElectronicAppeal>(dto);
        var result = await _repository.CreateAsync(entity);
        return _mapper.Map<ElectronicAppealDto>(result);
    }

    public async Task<ElectronicAppealDto> MarkAsReadAsync(int id)
    {
        var appeal = await _repository.GetByIdAsync(id);
        if (appeal == null) throw new Exception("Not found");
        
        if (!appeal.IsRead)
        {
            appeal.IsRead = true;
            appeal.ReadAt = DateTime.UtcNow;
            await _repository.UpdateAsync(appeal);
        }
        return _mapper.Map<ElectronicAppealDto>(appeal);
    }

    public async Task<ElectronicAppealDto> MarkAsRepliedAsync(int id)
    {
        var appeal = await _repository.GetByIdAsync(id);
        if (appeal == null) throw new Exception("Not found");

        if (!appeal.IsReplied)
        {
            appeal.IsReplied = true;
            appeal.RepliedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(appeal);
        }

        return _mapper.Map<ElectronicAppealDto>(appeal);
    }
}