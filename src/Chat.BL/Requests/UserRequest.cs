namespace Chat.BL.Requests;

public static class UserRequest
{
    public sealed record CreateUser(string Username, string? FirstName = default, string? LastName = default);

    public sealed record UpdateUser(string Username, string? FirstName = default, string? LastName = default);
}