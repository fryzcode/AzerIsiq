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

    public async Task<Image> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

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
    
    public async Task<Image> UpdateImageAsync(int id, IFormFile file, int substationId)
    {
        var existingImage = await _imageRepository.GetByIdAsync(id);
        if (existingImage == null)
            throw new FileNotFoundException("Image not found");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        existingImage.ImageName = file.FileName;
        existingImage.ImageData = ms.ToArray();
        existingImage.SubstationId = substationId;

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
}