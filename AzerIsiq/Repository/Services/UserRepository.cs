using AzerIsiq.Data;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsRefreshTokenAsync()
    {
        return await _context.Users.AnyAsync(u => u.RefreshToken == string.Empty);
    }

    public async Task<User?> GetUserWithRolesAsync(int Id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task AddUserRoleAsync(int userId, int roleId)
    {
        _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
        await _context.SaveChangesAsync();
    }
    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.RoleName)
            .ToListAsync();
    }


    public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateResetTokenAsync(int userId, string resetToken, DateTime expiryTime)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.ResetToken = resetToken;
            user.ResetTokenExpiration = expiryTime;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<User?> GetByResetTokenAsync(string token)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);
    }

    public async Task UpdatePasswordAsync(int userId, string newPasswordHash)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.PasswordHash = newPasswordHash;
            user.ResetToken = null;
            user.ResetTokenExpiration = null;
            await _context.SaveChangesAsync();
        }
    }
}
