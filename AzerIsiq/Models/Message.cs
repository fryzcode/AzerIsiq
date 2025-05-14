namespace AzerIsiq.Models;

public class Message
{
    public int Id { get; set; }

    public int SenderId { get; set; }
    public int ReceiverId { get; set; } // null, если это групповое сообщение
    public string? GroupName { get; set; } // null, если это приватное сообщение

    public string Text { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // Навигационные свойства (если нужны)
    public User Sender { get; set; }
    public User? Receiver { get; set; }
    public bool IsRead { get; set; }
}
