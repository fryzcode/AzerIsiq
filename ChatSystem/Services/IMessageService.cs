using ChatSystem.Models;

namespace ChatSystem.Services;

public interface IMessageService
{
    Task SaveMessageAsync(string groupName, string sender, string message);
    Task SaveMessageAsync(int senderUserId, int receiverUserId, string text);
    Task<List<Message>> GetMessagesForGroupAsync(string groupName);
    Task MarkMessageAsReadAsync(int messageId);
    Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int recipientId);
    Task MarkMessagesAsReadAsync(int readerId, int senderId);
    Task<int> GetUnreadMessageCountAsync(int userId);
    Task<int> GetUnreadMessageCountFromUserAsync(int currentUserId, int fromUserId);
}