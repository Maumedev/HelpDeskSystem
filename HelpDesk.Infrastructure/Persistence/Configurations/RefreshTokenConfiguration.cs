using HelpDesk.Core.Modules.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
           .ValueGeneratedOnAdd();

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(rt => rt.JwtId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(rt => rt.UserId)
           .IsRequired()
           .HasMaxLength(450);

        builder.Property(rt => rt.AddedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(rt => rt.ExpiryDate)
            .IsRequired()
            .HasDefaultValueSql("DATEADD(day, 7, GETUTCDATE())");

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(rt => rt.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.HasIndex(rt => rt.JwtId);
        builder.HasIndex(rt => rt.UserId);
    }
}
