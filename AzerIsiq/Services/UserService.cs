using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<UserListDto>> GetAllUsersAsync(UserQueryParameters parameters)
    {
        var users = await _userRepository.GetUsersAsync(parameters);
        var totalCount = await _userRepository.GetUsersCountAsync(parameters);

        var userListDtos = _mapper.Map<List<UserListDto>>(users);

        return new PagedResultDto<UserListDto>
        {
            Items = userListDtos,
            TotalCount = totalCount,
            Page = parameters.Page,
            PageSize = parameters.PageSize
        };
    }
    
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserWithRolesAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
    
    public async Task BlockUserAsync(int userId, bool isBlocked)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        user.IsBlocked = isBlocked;

        await _userRepository.UpdateAsync(user);
    }
}
