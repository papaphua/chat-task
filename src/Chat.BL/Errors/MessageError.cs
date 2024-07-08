using Chat.BL.Abstractions.Errors;
using Chat.BL.Entities;

namespace Chat.BL.Errors;

public class MessageError
{
    public static readonly Error NotFound = Error.NotFound(
        $"{nameof(Message)}.{nameof(NotFound)}",
        "Message not found.");
}