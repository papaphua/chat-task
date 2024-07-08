using Chat.BL.Abstractions.Errors;

namespace Chat.BL.Errors;

public static class ChatError
{
    public static readonly Error NotFound = Error.NotFound(
        $"{nameof(Entities.Chat)}.{nameof(NotFound)}",
        "Chat not found.");

    public static readonly Error NameRequired = Error.Validation(
        $"{nameof(Entities.Chat)}.{nameof(NameRequired)}",
        "Name is required.");

    public static readonly Error NotMember = Error.Validation(
        $"{nameof(Entities.Chat)}.{nameof(NotMember)}",
        "You don't have access to this chat.");

    public static readonly Error NoPermission = Error.Validation(
        $"{nameof(Entities.Chat)}.{nameof(NoPermission)}",
        "You don't have permission to perform this action.");

    public static readonly Error InternalError = Error.Internal(
        $"{nameof(Entities.Chat)}.{nameof(InternalError)}",
        "An internal error occurred while processing the chat.");

    public static readonly Error AlreadyMember = Error.Conflict(
        $"{nameof(Entities.Chat)}.{nameof(AlreadyMember)}",
        "You are already a member of this chat.");

    public static readonly Error OwnerLeaveError = Error.Validation(
        $"{nameof(Entities.Chat)}.{nameof(OwnerLeaveError)}",
        "Chat owner can only remove chat to leave it.");
}