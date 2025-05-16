using Grpc.Core;
using GrpcService;

namespace GrpcService.Services;

public class UserGrpcService : UserGrpc.UserGrpcBase
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<UserResponse> GetUserById(UserRequest request, ServerCallContext context)
    {
        var user = await _userService.GetByIdAsync(Guid.Parse(request.Id));
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        return new UserResponse
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Role = user.Role
        };
    }
}
