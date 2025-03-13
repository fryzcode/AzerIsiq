using AzerIsiq.Models;

namespace AzerIsiq.Dtos;

public class AuthResponseDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();

}