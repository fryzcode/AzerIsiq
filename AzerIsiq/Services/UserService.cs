using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
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
    
    public async Task AssignRoleToUserAsync(AssignRoleDto dto)
    {
        var user = await _userRepository.GetUserWithRolesAsync(dto.UserId)
                   ?? throw new Exception("User not found");

        var role = await _userRepository.GetRoleByNameAsync(dto.RoleName)
                   ?? throw new Exception("Role not found");

        var alreadyAssigned = user.UserRoles.Any(ur => ur.RoleId == role.Id);
        if (alreadyAssigned)
            throw new Exception("Role already assigned to user");

        var userRole = new UserRole()
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        await _userRepository.AddUserRoleAsync(userRole);
    }

}
