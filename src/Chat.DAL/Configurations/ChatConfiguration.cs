using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public sealed class ChatConfiguration : IEntityTypeConfiguration<BL.Entities.Chat>
{
    public void Configure(EntityTypeBuilder<BL.Entities.Chat> builder)
    {
        builder.HasOne(chat => chat.Owner)
            .WithMany()
            .HasForeignKey(chat => chat.OwnerId);

        builder.HasMany(chat => chat.Memberships)
            .WithOne(membership => membership.Chat)
            .HasForeignKey(membership => membership.ChatId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}