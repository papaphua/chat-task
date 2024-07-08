using Chat.BL.Abstractions.Errors;
using Chat.BL.Entities;

namespace Chat.BL.Errors;

public static class UserError
{
    public static readonly Error NotFound = Error.NotFound(
        $"{nameof(User)}.{nameof(NotFound)}",
        "User not found.");

    public static readonly Error AlreadyExists = Error.Conflict(
        $"{nameof(User)}.{nameof(AlreadyExists)}",
        "User already exists.");

    public static readonly Error UsernameRequired = Error.Validation(
        $"{nameof(User)}.{nameof(UsernameRequired)}",
        "Username is required.");
}