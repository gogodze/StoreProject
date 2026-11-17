using Domain.Aggregates;
using EntityFrameworkCore.DataProtection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email).IsEncryptedQueryable().HasMaxLength(100).IsRequired();
        builder.Property(x => x.FullName).IsEncrypted().HasMaxLength(100).IsRequired();
        builder.Property(x => x.HashedPassword).IsEncrypted().HasMaxLength(200).IsRequired();
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.RefreshToken).IsEncrypted().IsRequired(false);
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.AddressLine1).IsEncrypted().HasMaxLength(100).IsRequired();
            address.Property(a => a.AddressLine2).IsEncrypted().HasMaxLength(100).IsRequired(false);
            address.Property(a => a.City).IsEncrypted().HasMaxLength(50).IsRequired();
            address.Property(a => a.Country).IsEncrypted().HasMaxLength(56).IsRequired();
            address.Property(a => a.State).IsEncrypted().HasMaxLength(50).IsRequired(false);
            address.Property(a => a.ZipCode).HasMaxLength(20).IsRequired();
        });

        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
