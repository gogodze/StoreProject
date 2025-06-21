using System.Security.Claims;
using Application.Services;
using Domain.Aggregates;
using Domain.Common;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public sealed class HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IAppDbContext dbContext) : ICurrentUserAccessor
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Ulid? Id => User?.GetId();

    public Role? Role => User?.GetRole();

    public async Task<User?> TryGetCurrentUserAsync(CancellationToken ct = default)
    {
        if (Id is not { } id)
            return null;

        var user = await dbContext.Set<User>()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync(ct);

        return user ?? throw new InvalidOperationException($"Failed to load the user from the database, user with id: {id} not found");
    }
}