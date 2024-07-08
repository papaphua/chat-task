namespace Chat.BL.Requests;

public static class ChatRequest
{
    public sealed record CreateChat(string Name, string? Description = default);

    public sealed record UpdateChat(string Name, string? Description = default);
}