using AzerIsiq.Dtos;
using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface IUserService
{
    Task<PagedResultDto<UserListDto>> GetAllUsersAsync(UserQueryParameters parameters);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task BlockUserAsync(int userId, bool isBlocked);
    Task<List<RoleDto>> GetAllRolesAsync();
    Task AssignRolesToUserAsync(AssignRoleDto dto);
    Task<List<User>> GetAllUsersExceptAsync(int currentUserId);
}