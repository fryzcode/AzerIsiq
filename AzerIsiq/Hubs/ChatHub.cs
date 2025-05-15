using System.Security.Claims;
using AzerIsiq.Data;
using Microsoft.AspNetCore.SignalR;
using AzerIsiq.Services.ILogic;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IUserService _userService;
    private static readonly Dictionary<int, HashSet<string>> _onlineUsers = new();

    public ChatHub(IChatService chatService, IUserService userService)
    {
        _chatService = chatService;
        _userService = userService;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdStr, out var userId))
        {
            lock (_onlineUsers)
            {
                if (!_onlineUsers.ContainsKey(userId))
                    _onlineUsers[userId] = new HashSet<string>();

                _onlineUsers[userId].Add(Context.ConnectionId);
            }

            // Уведомляем всех клиентов, что пользователь в сети
            await Clients.All.SendAsync("UserOnline", userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdStr, out var userId))
        {
            bool isOffline = false;

            lock (_onlineUsers)
            {
                if (_onlineUsers.TryGetValue(userId, out var connections))
                {
                    connections.Remove(Context.ConnectionId);
                    if (connections.Count == 0)
                    {
                        _onlineUsers.Remove(userId);
                        isOffline = true;
                    }
                }
            }

            if (isOffline)
            {
                // Уведомляем всех клиентов, что пользователь ушёл
                await Clients.All.SendAsync("UserOffline", userId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public Task<List<int>> GetOnlineUserIds()
    {
        var ids = _onlineUsers.Keys.ToList();
        return Task.FromResult(ids);
    }

    public async Task Typing(int recipientUserId)
    {
        var senderIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(senderIdStr, out var senderId)) return;

        await Clients.User(recipientUserId.ToString())
            .SendAsync("UserTyping", senderId);
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
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
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
            
            await _chatService.MarkMessagesAsReadAsync(senderId, recipientUserId);

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

    public async Task<Dictionary<int, int>> GetUnreadCountsForAllUsers()
    {
        var currentUserIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(currentUserIdStr, out var currentUserId))
            return new Dictionary<int, int>();

        var users = await _userService.GetAllUsersExceptAsync(currentUserId);

        var result = new Dictionary<int, int>();

        foreach (var user in users)
        {
            var count = await _chatService.GetUnreadMessageCountFromUserAsync(currentUserId, user.Id);
            result[user.Id] = count;
        }

        return result;
    }

    public async Task<int> GetUnreadCountFromUser(int fromUserId)
    {
        var currentUserIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(currentUserIdStr, out var currentUserId)) return 0;

        return await _chatService.GetUnreadMessageCountFromUserAsync(currentUserId, fromUserId);
    }

    public async Task<int> GetTotalUnreadCount()
    {
        var currentUserIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(currentUserIdStr, out var currentUserId)) return 0;

        return await _chatService.GetUnreadMessageCountAsync(currentUserId);
    }

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