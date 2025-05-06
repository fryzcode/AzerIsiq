using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Models;

public class ElectronicAppeal
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public AppealTopic Topic { get; set; }
    public string Content { get; set; } = null!;

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    public bool IsReplied { get; set; } = false;
    public DateTime? RepliedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
