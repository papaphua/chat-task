namespace Chat.BL.Requests;

public static class MessageRequest
{
    public sealed record CreateMessage(string Text);
}