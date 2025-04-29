using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email).HasMaxLength(20).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(15).IsRequired();
        builder.Property(x => x.HashedPassword).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Role).IsRequired();
        builder.ComplexProperty(x => x.Address).IsRequired();
        builder.Property(x => x.RefreshToken).IsRequired(false);
        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}