using AzerIsiq.Repository.Interface;
using ChatSystem.Abstraction;
using ChatSystem.Models;

namespace ChatSystem.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task SaveMessageAsync(int senderUserId, int receiverUserId, string text)
    {
        var sender = await _userRepository.GetByIdAsync(senderUserId);
        var receiver = await _userRepository.GetByIdAsync(receiverUserId);

        if (sender == null) throw new Exception("Sender not found");
        if (receiver == null) throw new Exception("Receiver not found");

        var message = new Message()
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Text = text,
            IsRead = false,
            SentAt = DateTime.UtcNow
        };

        await _messageRepository.SaveMessageAsync(message);
    }
    
    public async Task SaveMessageAsync(string groupName, string senderUserName, string text)
    {
        var sender = await _userRepository.GetByEmailAsync(senderUserName);
        if (sender == null) throw new Exception("Sender not found");

        var message = new Message
        {
            SenderId = sender.Id,
            GroupName = groupName,
            Text = text,
            IsRead = false,
            SentAt = DateTime.UtcNow
        };

        await _messageRepository.SaveMessageAsync(message);
    }
    
    public async Task<List<Message>> GetMessagesBetweenUsersAsync(string senderUserName, string recipientUserId)
    {
        var sender = await _userRepository.GetByEmailAsync(senderUserName);
        var recipient = await _userRepository.GetByEmailAsync(recipientUserId);

        if (sender == null || recipient == null) throw new Exception("Sender or recipient not found");

        return await _messageRepository.GetMessagesBetweenUsersAsync(sender.Id, recipient.Id);
    }
    
    public async Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int recipientId)
    {
        return await _messageRepository.GetMessagesBetweenUsersAsync(senderId, recipientId);
    }

    public async Task<List<Message>> GetMessagesForGroupAsync(string groupName)
    {
        return await _messageRepository.GetMessagesForGroupAsync(groupName);
    }

    public async Task MarkMessagesAsReadAsync(int readerId, int senderId)
    {
        await _messageRepository.MarkMessagesAsReadAsync(readerId, senderId);
    }
    public async Task MarkMessageAsReadAsync(int messageId)
    {
        await _messageRepository.MarkAsReadAsync(messageId);
    }
    
    public Task<int> GetUnreadMessageCountAsync(int userId)
    {
        return _messageRepository.GetUnreadMessageCountAsync(userId);
    }

    public Task<int> GetUnreadMessageCountFromUserAsync(int currentUserId, int fromUserId)
    {
        return _messageRepository.GetUnreadMessageCountFromUserAsync(currentUserId, fromUserId);
    }
}