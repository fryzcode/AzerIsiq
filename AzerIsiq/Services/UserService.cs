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
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<UserListDto>> GetAllUsersAsync(UserQueryParameters parameters)
    {
        var users = await _userRepository.GetUsersPagedAsync(parameters);

        var userListDtos = _mapper.Map<List<UserListDto>>(users.Items);

        return new PagedResultDto<UserListDto>
        {
            Items = userListDtos,
            TotalCount = users.TotalCount,
            Page = parameters.Page,
            PageSize = parameters.PageSize
        };
    }
    
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserWithRolesAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
    
    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }
    
    public async Task BlockUserAsync(int userId, bool isBlocked)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        user.IsBlocked = isBlocked;

        await _userRepository.UpdateAsync(user);
    }
}
