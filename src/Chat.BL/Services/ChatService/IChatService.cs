using Chat.BL.Abstractions.Results;
using Chat.BL.Requests;

namespace Chat.BL.Services.ChatService;

public interface IChatService
{
    Task<Result<Entities.Chat>> GetChatAsync(Guid userId, Guid chatId);
    
    Task<Result<List<Entities.Chat>>> GetAllUserChatsAsync(Guid userId);
    
    Task<Result<Entities.Chat>> CreateChatAsync(Guid userId, ChatRequest.Create request);
    
    Task<Result<Entities.Chat>> UpdateChatAsync(Guid userId, Guid chatId, ChatRequest.Update request);

    Task<Result> RemoveChatAsync(Guid userId, Guid chatId);
}