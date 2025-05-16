using ChatSystem.Data;
using ChatSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSystem.Abstraction;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;
    
    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveMessageAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessagesForGroupAsync(string groupName)
    {
        return await _context.Messages
            .Where(m => m.GroupName == groupName)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
    
    public async Task<List<Message>> GetMessagesBetweenUsersAsync(int userId1, int userId2)
    {
        return await _context.Messages
            .Where(m =>
                (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
    
    
    public async Task MarkAsReadAsync(int messageId)
    {
        var msg = await _context.Messages.FindAsync(messageId);
        if (msg != null)
        {
            msg.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
    public async Task MarkMessagesAsReadAsync(int readerId, int senderId)
    {
        var messages = await _context.Messages
            .Where(m => m.SenderId == senderId && m.ReceiverId == readerId && !m.IsRead)
            .ToListAsync();

        foreach (var message in messages)
        {
            message.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<int> GetUnreadMessageCountAsync(int userId)
    {
        return await _context.Messages
            .Where(m => m.ReceiverId == userId && !m.IsRead)
            .CountAsync();
    }

    public async Task<int> GetUnreadMessageCountFromUserAsync(int currentUserId, int fromUserId)
    {
        return await _context.Messages
            .Where(m => m.ReceiverId == currentUserId && m.SenderId == fromUserId && !m.IsRead)
            .CountAsync();
    }
}