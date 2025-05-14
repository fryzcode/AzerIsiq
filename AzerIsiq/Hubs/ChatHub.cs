using System.Security.Claims;
using AzerIsiq.Data;
using Microsoft.AspNetCore.SignalR;
using AzerIsiq.Services.ILogic;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IUserService _userService;


    public ChatHub(IChatService chatService, IUserService userService)
    {
        _chatService = chatService;
        _userService = userService;
    }
    
    public async Task SendMessage(int recipientUserId, string message)
    {
        try
        {
            var senderIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(senderIdStr, out var senderId))
                return;
        
            Console.WriteLine($"📨 SendMessage: {senderId} -> {recipientUserId}: {message}");

            await _chatService.SaveMessageAsync(senderId, recipientUserId, message);
            await Clients.User(recipientUserId.ToString())
                .SendAsync("ReceiveMessage", senderId, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в SendMessage: {ex.Message}");
            throw;
        }

    }
    
    public async Task GetMessagesWithUser(int recipientUserId)
    {
        try
        {
            var senderIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(senderIdStr, out var senderId))
            {
                Console.WriteLine($"❌ Invalid sender ID: {senderIdStr}");
                return;
            }

            var messages = await _chatService.GetMessagesBetweenUsersAsync(senderId, recipientUserId);

            await Clients.Caller.SendAsync("LoadMessages", messages);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in GetMessagesWithUser: {ex}");
            throw;
        }
    }
    
    public async Task<List<ChatUserDto>> GetAllUsersExceptMe()
    {
        var currentUserIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(currentUserIdStr, out var currentUserId))
            return new List<ChatUserDto>();

        var users = await _userService.GetAllUsersExceptAsync(currentUserId);

        var result = users.Select(u => new ChatUserDto
        {
            Id = u.Id,
            UserName = u.UserName
        }).ToList();

        return result;
    }

    // public async Task SendMessage(string groupName, string message)
    // {
    //     var sender = Context.User?.Identity?.Name;
    //
    //     if (string.IsNullOrEmpty(sender))
    //         return;
    //
    //     await _chatService.SaveMessageAsync(groupName, sender, message);
    //
    //     await Clients.Group(groupName).SendAsync("ReceiveMessage", sender, message);
    // }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User?.Identity?.Name} joined {groupName}");
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User?.Identity?.Name} left {groupName}");
    }
    
    public async Task MarkAsRead(int messageId)
    {
        await _chatService.MarkMessageAsReadAsync(messageId);
    }
}