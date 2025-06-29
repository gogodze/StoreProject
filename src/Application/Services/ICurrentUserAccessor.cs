using Domain.Aggregates;
using Domain.ValueObjects;

namespace Application.Services;

public interface ICurrentUserAccessor
{
    public Ulid? Id { get; }

    public Role? Role { get; }
    public Task<User?> TryGetCurrentUserAsync(CancellationToken ct = default);

    public async Task<User> GetCurrentUserAsync(CancellationToken ct = default) =>
        await TryGetCurrentUserAsync(ct) ?? throw new InvalidOperationException("The user is not authenticated");
}