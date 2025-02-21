using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface
{
    public interface IRoleRepository
    {
        Task<Role?> GetByRoleNameAsync(string roleName);
    }
}
