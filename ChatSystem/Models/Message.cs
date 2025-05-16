using AzerIsiq.Models;

namespace ChatSystem.Models;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string? GroupName { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public User Sender { get; set; }
    public User? Receiver { get; set; }
    public bool IsRead { get; set; }
}
