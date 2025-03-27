using Application.Interfaces;
using Domain.Aggregates;
using Domain.Common;
using Domain.Entities;
using Infrastructure.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
            entity.SetTableName(entity.GetTableName()?.ToSnakeCaseRename());

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder conventionsBuilder)
    {
        conventionsBuilder.Properties<Ulid>().HaveConversion<UlidToStringValueConverter>();
        base.ConfigureConventions(conventionsBuilder);
    }
}