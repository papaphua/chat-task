using Chat.BL.Abstractions.Data;
using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Errors;
using Chat.BL.Requests;
using Microsoft.EntityFrameworkCore;

namespace Chat.BL.Services.ChatService;

public sealed class ChatService(
    IDbContext db,
    IUnitOfWork unitOfWork)
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

    public async Task<Result<Entities.Chat>> CreateChatAsync(Guid userId, ChatRequest.CreateChat request)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Result<Entities.Chat>.Failure(UserError.NotFound);

        if (string.IsNullOrWhiteSpace(request.Name)) return Result<Entities.Chat>.Failure(ChatError.NameRequired);

        var chat = Entities.Chat.Create(
            request.Name,
            userId,
            request.Description);

        var membership = Membership.Create(
            userId,
            chat.Id);

        var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            await db.AddAsync(chat);
            await db.AddAsync(membership);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return Result<Entities.Chat>.Failure(ChatError.InternalError);
        }

        await transaction.CommitAsync();

        // remove linked entities to avoid json cycles in response
        chat.Memberships = default;
        chat.Owner = default;

        return Result<Entities.Chat>.Success(chat);
    }

    public async Task<Result<Entities.Chat>> UpdateChatAsync(Guid userId, Guid chatId, ChatRequest.UpdateChat request)
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
        await unitOfWork.SaveChangesAsync();

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

        var memberships = await db.Set<Membership>()
            .Where(m => m.ChatId == chatId)
            .ToListAsync();

        var messages = await db.Set<Message>()
            .Where(m => m.ChatId == chatId)
            .ToListAsync();

        var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            db.RemoveRange(messages);
            db.RemoveRange(memberships);
            db.Remove(chat);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return Result.Failure(ChatError.InternalError);
        }

        await transaction.CommitAsync();

        return Result.Success();
    }

    public async Task<Result<List<Entities.Chat>>> SearchChatsAsync(string search)
    {
        var chats = await db.Set<Entities.Chat>()
            .Where(c => EF.Functions.Like(c.Name, $"%{search}%"))
            .ToListAsync();

        return Result<List<Entities.Chat>>.Success(chats);
    }

    public async Task<Result> JoinChatAsync(Guid userId, Guid chatId)
    {
        var chat = await db.Set<Entities.Chat>()
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat is null) return Result.Failure(ChatError.NotFound);

        var membership = await db.Set<Membership>()
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ChatId == chatId);

        if (membership is not null) return Result.Failure(ChatError.AlreadyMember);

        membership = Membership.Create(
            userId,
            chat.Id);

        await db.AddAsync(membership);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> LeaveChatAsync(Guid userId, Guid chatId)
    {
        var chat = await db.Set<Entities.Chat>()
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat is null) return Result.Failure(ChatError.NotFound);

        if (chat.OwnerId == userId) return Result.Failure(ChatError.OwnerLeaveError);

        var membership = await db.Set<Membership>()
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ChatId == chatId);

        if (membership is null) return Result.Failure(ChatError.NotMember);

        db.Remove(membership);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}