using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Requests;

namespace Chat.BL.Services.MessageService;

public interface IMessageService
{
    Task<Result<Message>> GetMessageAsync(Guid userId, Guid messageId);

    Task<Result<List<Message>>> GetAllMessagesAsync(Guid userId, Guid chatId);

    Task<Result<Message>> CreateMessageAsync(Guid userId, Guid chatId, MessageRequest.CreateMessage request);

    Task<Result> RemoveMessageAsync(Guid userId, Guid messageId);
}