using Application.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using EntityFrameworkCore.DataProtection.Extensions;
using Infrastructure.Persistence.Configurations;
using Infrastructure.ValueConverters;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class StoreDbContext(
    DbContextOptions<StoreDbContext> options,
    IDataProtectionProvider dataProtectionProvider) : DbContext(options), IAppDbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseDataProtection(dataProtectionProvider);
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
            entity.SetTableName(entity.GetTableName()?.ToSnakeCaseRename());

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OrderProductConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder conventionsBuilder)
    {
        conventionsBuilder.Properties<Ulid>().HaveConversion<UlidToStringValueConverter>();
        conventionsBuilder.Properties<RefreshToken>().HaveConversion<RefreshTokenToStringValueConverter>();
    }
}