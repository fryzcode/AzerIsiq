using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IImageRepository
{
    Task<ImageEntity> AddAsync(ImageEntity image);
    Task<ImageEntity?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}