using AzerIsiq.Data;
using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

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

    public async Task<User?> GetUserWithRolesAsync(int id)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
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
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);
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
    public async Task<PagedResultDto<User>> GetUsersPagedAsync(UserQueryParameters parameters)
    {
        var query = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var searchLower = parameters.Search.ToLower();
            query = query.Where(u =>
                u.Email.ToLower().Contains(searchLower) ||
                u.UserName.ToLower().Contains(searchLower) ||
                u.PhoneNumber.ToLower().Contains(searchLower) ||
                u.IpAddress.ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Role))
        {
            query = query.Where(u =>
                u.UserRoles.Any(ur => ur.Role.RoleName == parameters.Role));
        }

        if (parameters.IsBlocked.HasValue)
        {
            query = query.Where(u => u.IsBlocked == parameters.IsBlocked.Value);
        }

        if (parameters.CreatedAtFrom.HasValue)
        {
            query = query.Where(u => u.CreatedAt >= parameters.CreatedAtFrom.Value);
        }

        if (parameters.CreatedAtTo.HasValue)
        {
            query = query.Where(u => u.CreatedAt <= parameters.CreatedAtTo.Value);
        }
        
        int totalCount = await query.CountAsync();

        int skip = (parameters.Page - 1) * parameters.PageSize;
        var items = await query.Skip(skip).Take(parameters.PageSize).ToListAsync();

        return new PagedResultDto<User>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
