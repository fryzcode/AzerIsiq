using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface
{
    public interface IUserRepository
    {

        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task AddUserRoleAsync(int userId, int roleId);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime);
        Task<User?> GetUserWithRolesAsync(int id);
    }
}
