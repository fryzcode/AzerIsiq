using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;
using AzerIsiq.Models;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task SaveMessageAsync(int senderUserId, int receiverUserId, string text)
    {
        var sender = await _userRepository.GetByIdAsync(senderUserId);
        var receiver = await _userRepository.GetByIdAsync(receiverUserId);

        if (sender == null) throw new Exception("Sender not found");
        if (receiver == null) throw new Exception("Receiver not found");

        var message = new Message
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Text = text,
            IsRead = false,
            SentAt = DateTime.UtcNow
        };

        await _chatRepository.SaveMessageAsync(message);
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

        await _chatRepository.SaveMessageAsync(message);
    }
    
    public async Task<List<Message>> GetMessagesBetweenUsersAsync(string senderUserName, string recipientUserId)
    {
        var sender = await _userRepository.GetByEmailAsync(senderUserName);
        var recipient = await _userRepository.GetByEmailAsync(recipientUserId);

        if (sender == null || recipient == null) throw new Exception("Sender or recipient not found");

        return await _chatRepository.GetMessagesBetweenUsersAsync(sender.Id, recipient.Id);
    }
    
    public async Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int recipientId)
    {
        return await _chatRepository.GetMessagesBetweenUsersAsync(senderId, recipientId);
    }

    public async Task<List<Message>> GetMessagesForGroupAsync(string groupName)
    {
        return await _chatRepository.GetMessagesForGroupAsync(groupName);
    }

    public async Task MarkMessageAsReadAsync(int messageId)
    {
        await _chatRepository.MarkAsReadAsync(messageId);
    }
}