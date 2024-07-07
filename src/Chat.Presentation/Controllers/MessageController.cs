using Chat.BL.Services.MessageService;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/message")]
public sealed class MessageController(IMessageService messageService)
{
}