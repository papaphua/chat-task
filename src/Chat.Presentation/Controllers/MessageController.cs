using Chat.BL.Requests;
using Chat.BL.Services.MessageService;
using Chat.Presentation.Extensions;
using Chat.Presentation.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/message")]
public sealed class MessageController(
    IMessageService messageService,
    IHubContext<ChatHub> chatHubContext)
{
    [HttpGet("{userId:guid}/{messageId:guid}")]
    public async Task<IResult> GetMessage(Guid userId, Guid messageId)
    {
        var result = await messageService.GetMessageAsync(userId, messageId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpGet("{userId:guid}/{chatId:guid}/all")]
    public async Task<IResult> GetAllMessages(Guid userId, Guid chatId)
    {
        var result = await messageService.GetAllMessagesAsync(userId, chatId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPost("{userId:guid}/{chatId:guid}")]
    public async Task<IResult> CreateMessage(Guid userId, Guid chatId, MessageRequest.CreateMessage request)
    {
        var result = await messageService.CreateMessageAsync(userId, chatId, request);

        if (!result.IsSuccess) return result.ToProblemDetails();

        await chatHubContext.Clients.Group(chatId.ToString()).SendAsync(ChatHub.ReceiveMessage, result.Value);

        return Results.Ok(result.Value);
    }

    [HttpDelete("{userId:guid}/{messageId:guid}")]
    public async Task<IResult> RemoveMessage(Guid userId, Guid messageId)
    {
        var result = await messageService.RemoveMessageAsync(userId, messageId);

        return result.IsSuccess
            ? Results.Ok()
            : result.ToProblemDetails();
    }
}