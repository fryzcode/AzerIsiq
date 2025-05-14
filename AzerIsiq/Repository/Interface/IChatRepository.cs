using AzerIsiq.Models;

namespace AzerIsiq.Repository.Interface;

public interface IChatRepository
{
    Task SaveMessageAsync(Message message);
    Task<List<Message>> GetMessagesBetweenUsersAsync(int userId1, int userId2);
    Task<List<Message>> GetMessagesForGroupAsync(string groupName);
    Task MarkAsReadAsync(int messageId);
}