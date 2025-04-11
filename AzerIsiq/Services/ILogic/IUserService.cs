using AzerIsiq.Dtos;

namespace AzerIsiq.Services.ILogic;

public interface IUserService
{
    Task<PagedResultDto<UserListDto>> GetAllUsersAsync(UserQueryParameters parameters);
}