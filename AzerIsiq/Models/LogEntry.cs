namespace AzerIsiq.Models;

public class LogEntry
{
    public int Id { get; set; }
    public string Action { get; set; } 
    public string EntityName { get; set; }
    public int EntityId { get; set; } 
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}