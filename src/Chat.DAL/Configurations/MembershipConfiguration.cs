using Chat.BL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(membership => new { membership.UserId, membership.ChatId });
    }
}