namespace Chat.BL.Entities;

public sealed class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SenderId { get; set; }

    public User Sender { get; set; } = default!;

    public Guid ChatId { get; set; }

    public Chat Chat { get; set; } = default!;

    public string Text { get; set; } = default!;

    public static Message Create(Guid senderId, Guid chatId, string text)
    {
        return new Message
        {
            SenderId = senderId,
            ChatId = chatId,
            Text = text
        };
    }
}