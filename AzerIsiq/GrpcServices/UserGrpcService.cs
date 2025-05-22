using Azerisiq.Grpc;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;
using Grpc.Core;

namespace AzerIsiq.Services;

public class UserGrpcServiceImpl : UserGrpcService.UserGrpcServiceBase
{
    private readonly IUserRepository _userRepository;

    public UserGrpcServiceImpl(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<UserMessage> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

            return new UserMessage
            {
                Id = user.Id,
                FullName = user.UserName,
                Email = user.Email,
                IsBlocked = user.IsBlocked
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, $"Internal error: {ex.Message}"));
        }
        
    }

    public override async Task<UserMessage> GetUserByEmail(GetUserByEmailRequest request, ServerCallContext context)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        return new UserMessage
        {
            Id = user.Id,
            FullName = user.UserName,
            Email = user.Email,
            IsBlocked = user.IsBlocked
        };
    }
    
    public override async Task<UserListResponse> GetAllUsersExcept(GetAllUsersExceptRequest request, ServerCallContext context)
    {
        var users = await _userRepository.GetAllAsync();
        var filteredUsers = users.Where(u => u.Id != request.CurrentUserId)
            .Select(u => new UserMessage
            {
                Id = u.Id,
                FullName = u.UserName,
                Email = u.Email,
            })
            .ToList();

        var response = new UserListResponse();
        response.Users.AddRange(filteredUsers);

        return response;
    }

}