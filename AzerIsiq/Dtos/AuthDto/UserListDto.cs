
namespace AzerIsiq.Dtos;

public class UserListDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string IpAddress { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<string> UserRoles { get; set; } = new();
    public int FailedAttempts { get; set; } = 0; 
    public DateTime? LastFailedAttempt { get; set; }
}