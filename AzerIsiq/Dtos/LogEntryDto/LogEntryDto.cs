using System.ComponentModel.DataAnnotations.Schema;

namespace AzerIsiq.Dtos.LogEntryDto;

public class LogEntryDto
{
    public int Id { get; set; }
    public string EntryName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public int EntryId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<string> UserRoles { get; set; } = new();
    public DateTime Timestamp { get; set; }
}


