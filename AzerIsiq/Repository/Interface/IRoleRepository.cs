using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByRoleNameAsync(string roleName);
    }
}
