using Domain.Aggregates;
using EntityFrameworkCore.DataProtection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email).IsEncryptedQueryable().HasMaxLength(40).IsRequired();
        builder.Property(x => x.FullName).IsEncryptedQueryable().HasMaxLength(15).IsRequired();
        builder.Property(x => x.HashedPassword).IsEncryptedQueryable().HasMaxLength(50).IsRequired();
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.RefreshToken).IsEncryptedQueryable().IsRequired(false);
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.AddressLine1).IsEncryptedQueryable().HasMaxLength(100).IsRequired();
            address.Property(a => a.AddressLine2).IsEncryptedQueryable().HasMaxLength(100).IsRequired(false);
            address.Property(a => a.City).IsEncryptedQueryable().HasMaxLength(15).IsRequired();
            address.Property(a => a.Country).IsEncryptedQueryable().HasMaxLength(10).IsRequired();
            address.Property(a => a.State).IsEncryptedQueryable().HasMaxLength(10).IsRequired();
            address.Property(a => a.ZipCode).IsRequired();
        });

        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}