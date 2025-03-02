namespace AzerIsiq.Models;

public class OtpCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public int RequestCount { get; set; } = 0;
    public DateTime LastRequestTime { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}