using System.Security.Cryptography;
using Domain.Aggregates;

namespace Domain.ValueObjects;

public sealed record RefreshToken(string Token, DateTime? ExpireTime)
{
    public static RefreshToken CreateNew()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var str = Convert.ToBase64String(randomNumber);
        return new(str, DateTime.UtcNow.AddDays(7));
    }

    public static User ValidateToken(User user, RefreshToken refreshToken)
    {
        if (user.RefreshToken!.Token != refreshToken.Token || user.RefreshToken!.ExpireTime <= DateTime.UtcNow)
        {
            return null!;
        }

        return user;
    }
};