using Chat.BL.Abstractions.Data;
using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Errors;
using Chat.BL.Requests;
using Microsoft.EntityFrameworkCore;

namespace Chat.BL.Services.ChatService;

public sealed class ChatService(
    IDbContext db,
    IUnityOfWork unityOfWork)
    : IChatService
{
    public async Task<Result<Entities.Chat>> GetChatAsync(Guid userId, Guid chatId)
    {
        var chat = await db.Set<Membership>()
            .Where(m => m.UserId == userId && m.ChatId == chatId)
            .Select(m => m.Chat)
            .FirstOrDefaultAsync();

        return chat is null
            ? Result<Entities.Chat>.Failure(ChatError.NotFound)
            : Result<Entities.Chat>.Success(chat);
    }

    public async Task<Result<List<Entities.Chat>>> GetAllUserChatsAsync(Guid userId)
    {
        var chats = await db.Set<Membership>()
            .Where(m => m.UserId == userId)
            .Select(m => m.Chat)
            .ToListAsync();

        return Result<List<Entities.Chat>>.Success(chats);
    }

    public async Task<Result<Entities.Chat>> CreateChatAsync(Guid userId, ChatRequest.Create request)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Result<Entities.Chat>.Failure(UserError.NotFound);

        if (string.IsNullOrWhiteSpace(request.Name)) return Result<Entities.Chat>.Failure(ChatError.NameRequired);

        var chat = Entities.Chat.Create(
            request.Name,
            userId,
            request.Description);

        await db.AddAsync(chat);
        await unityOfWork.SaveChangesAsync();

        return Result<Entities.Chat>.Success(chat);
    }

    public async Task<Result<Entities.Chat>> UpdateChatAsync(Guid userId, Guid chatId, ChatRequest.Update request)
    {
        var chat = await db.Set<Membership>()
            .Where(m => m.UserId == userId && m.ChatId == chatId)
            .Select(m => m.Chat)
            .FirstOrDefaultAsync();

        if (chat is null) return Result<Entities.Chat>.Failure(ChatError.NotFound);

        if (chat.OwnerId != userId) return Result<Entities.Chat>.Failure(ChatError.NoPermission);

        if (string.IsNullOrWhiteSpace(request.Name)) return Result<Entities.Chat>.Failure(ChatError.NameRequired);

        chat.Name = request.Name;
        chat.Description = request.Description;

        db.Update(chat);
        await unityOfWork.SaveChangesAsync();

        return Result<Entities.Chat>.Success(chat);
    }

    public async Task<Result> RemoveChatAsync(Guid userId, Guid chatId)
    {
        var chat = await db.Set<Membership>()
            .Where(m => m.UserId == userId && m.ChatId == chatId)
            .Select(m => m.Chat)
            .FirstOrDefaultAsync();

        if (chat is null) return Result.Failure(ChatError.NotFound);
        
        if (chat.OwnerId != userId) return Result.Failure(ChatError.NoPermission);
        
        db.Remove(chat);
        await unityOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
}