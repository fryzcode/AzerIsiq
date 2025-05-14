using AzerIsiq.Models;

namespace AzerIsiq.Services.ILogic;

public interface IChatService
{
    Task SaveMessageAsync(string groupName, string sender, string message);
    Task SaveMessageAsync(int senderUserId, int receiverUserId, string text);
    Task<List<Message>> GetMessagesForGroupAsync(string groupName);
    Task MarkMessageAsReadAsync(int messageId);
    // Task<List<Message>> GetMessagesBetweenUsersAsync(string senderUserName, string recipientUserId);
    Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int recipientId);
}