using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class OtpCodeRepository : GenericRepository<OtpCode>, IOtpCodeRepository
{
    private readonly AppDbContext _context;

    public OtpCodeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<OtpCode?> GetLatestOtpByUserIdAsync(int userId)
    {
        return await _context.OtpCodes
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.LastRequestTime)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetOtpRequestCountAsync(int userId)
    {
        return await _context.OtpCodes
            .Where(o => o.UserId == userId && o.LastRequestTime > DateTime.UtcNow.AddMinutes(-10))
            .CountAsync();
    }

    public async Task AddOtpAsync(OtpCode otp)
    {
        await _context.OtpCodes.AddAsync(otp);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteExpiredOtpsByUserAsync(int userId)
    {
        var expiredOtps = await _context.OtpCodes
            .Where(o => o.UserId == userId && o.Expiration < DateTime.UtcNow)
            .ToListAsync();

        if (expiredOtps.Any())
        {
            _context.OtpCodes.RemoveRange(expiredOtps);
            await _context.SaveChangesAsync();
        }
    }
}