using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<Image> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        const long maxFileSize = 5 * 1024 * 1024;
        if (file.Length > maxFileSize)
            throw new ArgumentException("File size exceeds the 5MB limit");

        var allowedMimeTypes = new HashSet<string> { "image/jpeg", "image/png", "image/jpg" };
        if (!allowedMimeTypes.Contains(file.ContentType))
            throw new ArgumentException("Only JPEG and PNG formats are allowed");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var imageData = ms.ToArray();

        var image = new Image
        {
            ImageData = imageData,
            ImageName = file.FileName
        };

        return await _imageRepository.AddAsync(image);
    }
    public async Task<Image> UpdateImageAsync(ImageUpdateDto dto)
    {
        var existingImage = await _imageRepository.GetByIdAsync(dto.Id);
        if (existingImage == null)
            throw new FileNotFoundException("Image not found");

        using var memoryStream = new MemoryStream();

        using var ms = new MemoryStream();
        await dto.File.CopyToAsync(ms);

        existingImage.ImageName = dto.File.FileName;
        existingImage.ImageData = ms.ToArray();

        if (dto.SubstationId.HasValue)
            existingImage.SubstationId = dto.SubstationId.Value;

        if (dto.TmId.HasValue)
            existingImage.TmId = dto.TmId.Value;
        
        await _imageRepository.UpdateAsync(existingImage);
        return existingImage;
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
    public async Task UpdateSubOrTmImageAsync(Image image)
    {
        var existingImage = await _imageRepository.GetByIdAsync(image.Id);
        if (existingImage == null)
            throw new FileNotFoundException("Image not found");

        existingImage.SubstationId = image.SubstationId;
        existingImage.TmId = image.TmId;
        await _imageRepository.UpdateAsync(existingImage);
    }
    public async Task<Image?> GetImageBySubstationIdAsync(int substationId)
    {
        return await _imageRepository.GetBySubstationIdAsync(substationId);
    }

}