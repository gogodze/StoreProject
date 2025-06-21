using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Services;

public interface IAppDbContext
{
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    public Task<int> SaveChangesAsync(CancellationToken ct = default);

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}