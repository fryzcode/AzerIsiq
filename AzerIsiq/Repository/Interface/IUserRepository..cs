using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsRefreshTokenAsync();
        public Task<List<string>> GetUserRolesAsync(int userId);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task AddUserRoleAsync(int userId, int roleId);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime);
        Task<User?> GetUserWithRolesAsync(int id);
        Task UpdateResetTokenAsync(int userId, string resetToken, DateTime expiryTime);
        Task<User?> GetByResetTokenAsync(string token);
        Task UpdatePasswordAsync(int userId, string newPasswordHash);
    }
}
