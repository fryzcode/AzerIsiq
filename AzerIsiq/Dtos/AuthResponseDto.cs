namespace AzerIsiq.Dtos;

public class AuthResponseDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public string Token { get; set; } = string.Empty;
}