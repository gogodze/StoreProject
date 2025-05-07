using Application.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence.Configurations;
using Infrastructure.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Db;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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