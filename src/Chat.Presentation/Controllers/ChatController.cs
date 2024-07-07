using Chat.BL.Requests;
using Chat.BL.Services.ChatService;
using Chat.Presentation.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/chat")]
public sealed class ChatController(IChatService chatService)
{
    [HttpGet("{userId:guid}/{chatId:guid}")]
    public async Task<IResult> GetChat(Guid userId, Guid chatId)
    {
        var result = await chatService.GetChatAsync(userId, chatId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpGet("{userId:guid}")]
    public async Task<IResult> GetAllUserChats(Guid userId)
    {
        var result = await chatService.GetAllUserChatsAsync(userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPost]
    public async Task<IResult> CreateChat(Guid userId, ChatRequest.CreateChat request)
    {
        var result = await chatService.CreateChatAsync(userId, request);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPut("{userId:guid}/{chatId:guid}")]
    public async Task<IResult> UpdateChat(Guid userId, Guid chatId, ChatRequest.UpdateChat request)
    {
        var result = await chatService.UpdateChatAsync(userId, chatId, request);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpDelete("{userId:guid}/{chatId:guid}")]
    public async Task<IResult> RemoveChat(Guid userId, Guid chatId)
    {
        var result = await chatService.RemoveChatAsync(userId, chatId);

        return result.IsSuccess
            ? Results.Ok()
            : result.ToProblemDetails();
    }

    [HttpGet]
    [Route("{search}")]
    public async Task<IResult> SearchChats(string search)
    {
        var result = await chatService.SearchChatsAsync(search);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPost("{userId:guid}/{chatId:guid}/join")]
    public async Task<IResult> JoinChat(Guid userId, Guid chatId)
    {
        var result = await chatService.JoinChatAsync(userId, chatId);

        return result.IsSuccess
            ? Results.Ok()
            : result.ToProblemDetails();
    }

    [HttpDelete("{userId:guid}/{chatId:guid}/leave")]
    public async Task<IResult> LeaveChat(Guid userId, Guid chatId)
    {
        var result = await chatService.LeaveChatAsync(userId, chatId);

        return result.IsSuccess
            ? Results.Ok()
            : result.ToProblemDetails();
    }
}