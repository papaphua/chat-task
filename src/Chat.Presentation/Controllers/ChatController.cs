using Chat.BL.Services.ChatService;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/chat")]
public sealed class ChatController(IChatService chatService)
{
}