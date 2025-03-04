using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ImageEntity> AddAsync(ImageEntity image)
    {
        _context.ImageEntities.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<ImageEntity?> GetByIdAsync(int id)
    {
        return await _context.ImageEntities.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var image = await _context.ImageEntities.FindAsync(id);
        if (image == null) return false;

        _context.ImageEntities.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }
}