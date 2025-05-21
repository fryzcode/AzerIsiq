// using Grpc.Core;
// using UserInfo.Grpc;
//
// namespace AzerIsiq.Services;
//
// public class UserGrpcService : UserService.UserServiceBase
// {
//     private readonly IUserRepository _userRepository;
//
//     public UserGrpcService(IUserRepository userRepository)
//     {
//         _userRepository = userRepository;
//     }
//
//     public override async Task<UserDto> GetUserById(GetUserByIdRequest request, ServerCallContext context)
//     {
//         var user = await _userRepository.GetByIdAsync(request.UserId);
//         if (user == null)
//         {
//             throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
//         }
//
//         return new UserDto
//         {
//             Id = user.Id.ToString(),
//             Username = user.Username,
//             Email = user.Email,
//             Role = user.Role
//         };
//     }
// }