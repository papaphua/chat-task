using Chat.BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(user => user.Memberships)
            .WithOne(membership => membership.User)
            .HasForeignKey(membership => membership.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}