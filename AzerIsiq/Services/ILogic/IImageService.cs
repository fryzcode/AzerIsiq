using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface IImageService
{
    Task<ImageEntity> UploadImageAsync(IFormFile file);
    Task<byte[]> GetImageBytesAsync(int id);
    Task<bool> DeleteImageAsync(int id);
}