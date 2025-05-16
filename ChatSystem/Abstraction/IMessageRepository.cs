using ChatSystem.Models;

namespace ChatSystem.Abstraction;

public interface IMessageRepository
{
    Task SaveMessageAsync(Message message);
    Task MarkMessagesAsReadAsync(int readerId, int senderId);
    Task<List<Message>> GetMessagesBetweenUsersAsync(int userId1, int userId2);
    Task<List<Message>> GetMessagesForGroupAsync(string groupName);
    Task MarkAsReadAsync(int messageId);
    Task<int> GetUnreadMessageCountAsync(int userId);
    Task<int> GetUnreadMessageCountFromUserAsync(int currentUserId, int fromUserId);
}