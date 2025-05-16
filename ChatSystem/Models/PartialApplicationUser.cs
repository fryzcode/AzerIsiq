namespace ChatSystem.Models;

public partial class User
{
    public ICollection<Message> SentMessages { get; set; }
    public ICollection<Message> ReceivedMessages { get; set; }
}