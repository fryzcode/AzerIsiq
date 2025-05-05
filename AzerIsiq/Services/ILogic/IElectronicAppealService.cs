using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface IElectronicAppealService
{
    Task<IEnumerable<ElectronicAppealDto>> GetAllAsync();
    Task<ElectronicAppealDto> GetByIdAsync(int id);
    Task<ElectronicAppealDto> CreateAsync(ElectronicAppealCreateDto dto);
    Task<ElectronicAppealDto> MarkAsReadAsync(int id);
    Task<ElectronicAppealDto> MarkAsRepliedAsync(int id);
}