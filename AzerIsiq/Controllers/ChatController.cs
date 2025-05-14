using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("{groupName}")]
    public async Task<IActionResult> GetMessages(string groupName)
    {
        var messages = await _chatService.GetMessagesForGroupAsync(groupName);
        return Ok(messages);
    }
}
