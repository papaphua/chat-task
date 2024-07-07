namespace Chat.BL.Requests;

public static class UserRequest
{
    public record Create(string Username, string? FirstName = default, string? LastName = default);

    public record Update(string Username, string? FirstName = default, string? LastName = default);
}