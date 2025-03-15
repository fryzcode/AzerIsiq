using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface IImageService
{
    Task<Image> UploadImageAsync(IFormFile file);
    Task<Image> UpdateImageAsync(ImageUpdateDto dto);
    Task<byte[]> GetImageBytesAsync(int id);
    Task<bool> DeleteImageAsync(int id);
    Task UpdateSubOrTmImageAsync(Image image);
    Task<Image?> GetImageBySubstationIdAsync(int substationId);
}