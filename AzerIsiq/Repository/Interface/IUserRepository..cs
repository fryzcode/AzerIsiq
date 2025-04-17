using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsRefreshTokenAsync();
    Task<List<string>> GetUserRolesAsync(int userId);
    Task AddUserRoleAsync(int userId, int roleId);
    Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime);
    Task<User?> GetUserWithRolesAsync(int id);
    Task UpdateResetTokenAsync(int userId, string resetToken, DateTime expiryTime);
    Task<User?> GetByResetTokenAsync(string token);
    Task UpdatePasswordAsync(int userId, string newPasswordHash);
    Task<PagedResultDto<User>> GetUsersPagedAsync(UserQueryParameters parameters);
    Task ResetFailedAttemptsAsync(CancellationToken cancellationToken);
}