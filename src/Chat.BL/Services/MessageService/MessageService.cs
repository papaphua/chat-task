using Chat.BL.Abstractions.Data;
using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Errors;
using Chat.BL.Requests;
using Microsoft.EntityFrameworkCore;

namespace Chat.BL.Services.MessageService;

public sealed class MessageService(
    IDbContext db,
    IUnityOfWork unityOfWork)
    : IMessageService
{
    public async Task<Result<Message>> GetMessageAsync(Guid userId, Guid messageId)
    {
        var message = await db.Set<Message>()
            .FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == userId);

        return message is null
            ? Result<Message>.Failure(MessageError.NotFound)
            : Result<Message>.Success(message);
    }

    public async Task<Result<List<Message>>> GetAllMessagesAsync(Guid userId, Guid chatId)
    {
        var membership = await db.Set<Membership>()
            .FirstOrDefaultAsync(m => m.UserId == userId && m.ChatId == chatId);

        if (membership is null) return Result<List<Message>>.Failure(ChatError.NotFound);

        var messages = await db.Set<Message>()
            .Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();

        return Result<List<Message>>.Success(messages);
    }

    public async Task<Result<Message>> CreateMessageAsync(Guid userId, Guid chatId,
        MessageRequest.CreateMessage request)
    {
        var membership = await db.Set<Membership>()
            .FirstOrDefaultAsync(m => m.UserId == userId && m.ChatId == chatId);

        if (membership is null) return Result<Message>.Failure(ChatError.NotFound);

        var message = Message.Create(
            userId,
            chatId,
            request.Text);

        await db.AddAsync(message);
        await unityOfWork.SaveChangesAsync();

        return Result<Message>.Success(message);
    }

    public async Task<Result> RemoveMessageAsync(Guid userId, Guid messageId)
    {
        var message = await db.Set<Message>()
            .FirstOrDefaultAsync(m => m.Id == messageId && m.SenderId == userId);

        if (message is null) return Result.Failure(MessageError.NotFound);

        db.Remove(message);
        await unityOfWork.SaveChangesAsync();

        return Result.Success();
    }
}