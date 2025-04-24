using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IAppDbContext
{
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    public Task<int> SaveChangesAsync(CancellationToken ct = default);
}