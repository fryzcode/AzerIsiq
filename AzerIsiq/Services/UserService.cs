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
    
    public async Task AssignRolesToUserAsync(AssignRoleDto dto)
    {
        var user = await _userRepository.GetUserWithRolesAsync(dto.UserId)
                   ?? throw new Exception("User not found");

        var allRoles = await _userRepository.GetRolesByNamesAsync(dto.RoleNames);
        if (allRoles.Count != dto.RoleNames.Count)
            throw new Exception("One or more roles not found");

        var currentRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
        var selectedRoleIds = allRoles.Select(r => r.Id).ToList();

        var rolesToAdd = selectedRoleIds.Except(currentRoleIds).ToList();

        var rolesToRemove = currentRoleIds.Except(selectedRoleIds).ToList();

        foreach (var roleId in rolesToAdd)
        {
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            };
            await _userRepository.AddUserRoleAsync(userRole);
        }

        foreach (var roleId in rolesToRemove)
        {
            await _userRepository.RemoveUserRoleAsync(user.Id, roleId);
        }
    }

    public async Task<List<User>> GetAllUsersExceptAsync(int currentUserId)
    {
        var allUsers = await _userRepository.GetAllAsync();
        return allUsers.Where(u => u.Id != currentUserId).ToList();
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
