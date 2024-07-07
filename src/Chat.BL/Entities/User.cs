namespace Chat.BL.Entities;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = default!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public ICollection<Membership> Memberships { get; set; } = default!;

    public static User Create(string username, string? firstName = default, string? lastName = default)
    {
        return new User
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName
        };
    }
}