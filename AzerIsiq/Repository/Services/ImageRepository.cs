using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Image> AddAsync(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<Image> UpdateAsync(Image image)
    {
        var existingImage = await _context.Images.FindAsync(image.Id);
        if (existingImage == null) return null;

        existingImage.ImageName = image.ImageName;
        existingImage.ImageData = image.ImageData;
        existingImage.SubstationId = image.SubstationId;
        existingImage.TmId = image.TmId;

        _context.Images.Update(existingImage);
        await _context.SaveChangesAsync();
        return existingImage;
    }
    
    public async Task<Image?> GetByIdAsync(int id)
    {
        return await _context.Images.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null) return false;

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Image?> GetBySubstationIdAsync(int substationId)
    {
        return await _context.Images.FirstOrDefaultAsync(i => i.SubstationId == substationId);
    }
}