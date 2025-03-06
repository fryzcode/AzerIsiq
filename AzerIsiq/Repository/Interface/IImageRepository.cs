using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IImageRepository
{
    Task<Image> AddAsync(Image image);
    Task<Image?> UpdateAsync(Image image);
    Task<Image?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<Image?> GetBySubstationIdAsync(int substationId);
}