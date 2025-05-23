using System.Security.Claims;
using System.Collections.Concurrent;
using AzerIsiq.Data;
using Microsoft.AspNetCore.SignalR;
using AzerIsiq.Services.ILogic;
using Microsoft.Extensions.Logging;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IUserService _userService;
    private readonly ILogger<ChatHub> _logger;

    private static readonly ConcurrentDictionary<int, ConcurrentHashSet<string>> _onlineUsers = new();

    public ChatHub(IChatService chatService, IUserService userService, ILogger<ChatHub> logger)
    {
        _chatService = chatService;
        _userService = userService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return;

        var connections = _onlineUsers.GetOrAdd(userId.Value, _ => new ConcurrentHashSet<string>());
        connections.SafeAdd(Context.ConnectionId);

        await Clients.All.SendAsync("UserOnline", userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return;

        if (_onlineUsers.TryGetValue(userId.Value, out var connections))
        {
            connections.SafeRemove(Context.ConnectionId);

            if (connections.SafeCount == 0)
            {
                _onlineUsers.TryRemove(userId.Value, out _);
                await Clients.All.SendAsync("UserOffline", userId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public Task<List<int>> GetOnlineUserIds()
    {
        return Task.FromResult(_onlineUsers.Keys.ToList());
    }

    public async Task Typing(int recipientUserId)
    {
        var senderId = GetCurrentUserId();
        if (senderId == null) return;

        await Clients.User(recipientUserId.ToString()).SendAsync("UserTyping", senderId.Value);
    }

    public async Task SendPrivateMessage(int recipientUserId, string message)
    {
        var senderId = GetCurrentUserId();
        if (senderId == null) return;

        try
        {
            _logger.LogInformation("📨 SendMessage: {SenderId} -> {RecipientId}: {Message}", senderId, recipientUserId, message);

            await _chatService.SaveMessageAsync(senderId.Value, recipientUserId, message);

            var messageDto = new
            {
                SenderId = senderId.Value,
                Text = message,
                Timestamp = DateTime.UtcNow
            };

            await Clients.User(recipientUserId.ToString()).SendAsync("ReceiveMessage", messageDto);
            await Clients.Caller.SendAsync("ReceiveMessage", messageDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendPrivateMessage");
            throw;
        }
    }

    public async Task GetMessagesWithUser(int recipientUserId)
    {
        var senderId = GetCurrentUserId();
        if (senderId == null)
        {
            _logger.LogWarning("Invalid sender ID in GetMessagesWithUser");
            return;
        }

        try
        {
            var messages = await _chatService.GetMessagesBetweenUsersAsync(senderId.Value, recipientUserId);
            await _chatService.MarkMessagesAsReadAsync(senderId.Value, recipientUserId);
            await Clients.Caller.SendAsync("LoadMessages", messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetMessagesWithUser");
            throw;
        }
    }

    public async Task<List<ChatUserDto>> GetAllUsersExceptMe()
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return new List<ChatUserDto>();

        var users = await _userService.GetAllUsersExceptAsync(currentUserId.Value);
        return users.Select(u => new ChatUserDto { Id = u.Id, UserName = u.UserName }).ToList();
    }

    public async Task<Dictionary<int, int>> GetUnreadCountsForAllUsers()
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return new Dictionary<int, int>();

        try
        {
            var users = await _userService.GetAllUsersExceptAsync(currentUserId.Value);
            var result = new Dictionary<int, int>();

            foreach (var user in users)
            {
                var count = await _chatService.GetUnreadMessageCountFromUserAsync(currentUserId.Value, user.Id);
                result[user.Id] = count;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUnreadCountsForAllUsers");
            return new Dictionary<int, int>();
        }
    }

    public async Task<int> GetUnreadCountFromUser(int fromUserId)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return 0;

        return await _chatService.GetUnreadMessageCountFromUserAsync(currentUserId.Value, fromUserId);
    }

    public async Task<int> GetTotalUnreadCount()
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null) return 0;

        return await _chatService.GetUnreadMessageCountAsync(currentUserId.Value);
    }

    public async Task JoinGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName)) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User?.Identity?.Name} joined {groupName}");
    }

    public async Task LeaveGroup(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName)) return;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User?.Identity?.Name} left {groupName}");
    }

    public async Task MarkAsRead(int messageId)
    {
        await _chatService.MarkMessageAsReadAsync(messageId);
    }
    
    private int? GetCurrentUserId()
    {
        var userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdStr, out var userId) ? userId : (int?)null;
    }
}
public class ConcurrentHashSet<T>
{
    private readonly HashSet<T> _set = new();
    private readonly object _lock = new();

    public void SafeAdd(T item)
    {
        lock (_lock)
        {
            _set.Add(item);
        }
    }

    public void SafeRemove(T item)
    {
        lock (_lock)
        {
            _set.Remove(item);
        }
    }

    public int SafeCount
    {
        get
        {
            lock (_lock)
            {
                return _set.Count;
            }
        }
    }
}
