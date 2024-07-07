using Chat.BL.Requests;
using Chat.BL.Services.MessageService;
using Chat.Presentation.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/message")]
public sealed class MessageController(IMessageService messageService)
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

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
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