using Chat.BL.Abstractions.Results;
using Chat.BL.Requests;

namespace Chat.BL.Services.ChatService;

public interface IChatService
{
    Task<Result<Entities.Chat>> GetChatAsync(Guid userId, Guid chatId);
    
    Task<Result<List<Entities.Chat>>> GetAllUserChatsAsync(Guid userId);
    
    Task<Result<Entities.Chat>> CreateChatAsync(Guid userId, ChatRequest.CreateChat request);
    
    Task<Result<Entities.Chat>> UpdateChatAsync(Guid userId, Guid chatId, ChatRequest.UpdateChat request);

    Task<Result> RemoveChatAsync(Guid userId, Guid chatId);

    Task<Result<List<Entities.Chat>>> SearchChatsAsync(string search);

    Task<Result> JoinChatAsync(Guid userId, Guid chatId);
    
    Task<Result> LeaveChatAsync(Guid userId, Guid chatId);
}