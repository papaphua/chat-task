using Chat.BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(message => message.Sender)
            .WithMany()
            .HasForeignKey(message => message.SenderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(message => message.Chat)
            .WithMany()
            .HasForeignKey(message => message.ChatId)
            .OnDelete(DeleteBehavior.NoAction);;
    }
}