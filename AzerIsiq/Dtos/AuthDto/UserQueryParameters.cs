namespace AzerIsiq.Dtos;

public class UserQueryParameters : PagedRequestDto
{
    public string? Search { get; set; } 
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IpAddress { get; set; }
    public bool? IsBlocked { get; set; }
    public string? Role { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
}
