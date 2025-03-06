using AzerIsiq.Models;

namespace AzerIsiq.Services;

public interface IImageService
{
    Task<Image> UploadImageAsync(IFormFile file);
    Task<Image> UpdateImageAsync(int id, IFormFile file, int substationId);
    Task<byte[]> GetImageBytesAsync(int id);
    Task<bool> DeleteImageAsync(int id);
    Task UpdateSubOrTmImageAsync(Image image);
}