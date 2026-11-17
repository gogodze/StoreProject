using Application.Services;
using Domain.Aggregates;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Client.Auth;

internal static class AdminBootstrap
{
    public static async Task EnsureAdministratorAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var hasAdmin = await db.Set<User>()
            .AnyAsync(u => u.Role == Role.Administrator);
        if (hasAdmin)
            return;

        var email = Environment.GetEnvironmentVariable("ADMIN__EMAIL") ?? "admin@store.local";
        var password = Environment.GetEnvironmentVariable("ADMIN__PASSWORD") ?? "Admin123!";
        var name = Environment.GetEnvironmentVariable("ADMIN__NAME") ?? "Store Admin";

        await db.Set<User>().AddAsync(new User
        {
            Id = Ulid.NewUlid(),
            Email = email,
            FullName = name,
            HashedPassword = EnhancedHashPassword(password),
            Role = Role.Administrator,
            RegisterDate = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
    }
}
