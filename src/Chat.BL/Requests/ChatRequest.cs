namespace Chat.BL.Requests;

public static class ChatRequest
{
    public record Create(string Name, string? Description = default);

    public record Update(string Name, string? Description = default);
}