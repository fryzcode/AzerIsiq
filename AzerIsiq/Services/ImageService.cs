using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<ImageEntity> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var imageData = ms.ToArray();

        var image = new ImageEntity
        {
            ImageData = imageData,
            ImageName = file.FileName
        };

        return await _imageRepository.AddAsync(image);
    }

    public async Task<byte[]> GetImageBytesAsync(int id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        return image?.ImageData ?? Array.Empty<byte>();
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        return await _imageRepository.DeleteAsync(id);
    }
}