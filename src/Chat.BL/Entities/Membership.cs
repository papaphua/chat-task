namespace Chat.BL.Entities;

public sealed class Membership
{
    public Guid UserId { get; set; }

    public User User { get; set; } = default!;

    public Guid ChatId { get; set; }

    public Chat Chat { get; set; } = default!;

    public static Membership Create(Guid userId, Guid chatId)
    {
        return new Membership
        {
            UserId = userId,
            ChatId = chatId
        };
    }
}