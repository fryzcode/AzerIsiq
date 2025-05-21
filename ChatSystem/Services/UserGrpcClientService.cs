using Azerisiq.Grpc;

namespace GrpcService.Services;

public class UserGrpcClientService
{
    private readonly UserGrpcService.UserGrpcServiceClient _client;

    public UserGrpcClientService(UserGrpcService.UserGrpcServiceClient client)
    {
        _client = client;
    }

    public async Task<UserMessage> GetByIdAsync(int id)
    {
        return await _client.GetUserByIdAsync(new GetUserByIdRequest { Id = id });
    }

    public async Task<UserMessage> GetByEmailAsync(string email)
    {
        return await _client.GetUserByEmailAsync(new GetUserByEmailRequest { Email = email });
    }

    public async Task<List<UserMessage>> GetAllUsersExceptAsync(int currentUserId)
    {
        var response = await _client.GetAllUsersExceptAsync(new GetAllUsersExceptRequest { CurrentUserId = currentUserId });
        return response.Users.ToList();
    }
}