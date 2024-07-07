namespace Chat.BL.Entities;

public sealed class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public Guid OwnerId { get; set; }

    public User Owner { get; set; } = default!;

    public ICollection<Membership> Memberships { get; set; } = default!;

    public static Chat Create(string name, Guid ownerId, string? description = default)
    {
        return new Chat
        {
            Name = name,
            OwnerId = ownerId,
            Description = description
        };
    }
}