using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Aggregates;
using Domain.Common;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public static class JwtGenerator
{
    private static readonly string ClaimsIssuer = "JWT__ISSUER".GetFromEnvRequired();
    private static readonly string ClaimsAudience = "JWT__AUDIENCE".GetFromEnvRequired();
    private static readonly string Key = "JWT__KEY".GetFromEnvRequired();

    public static readonly JwtBearerEvents Events = new()
    {
        OnChallenge = ctx =>
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.Found;
            ctx.Response.Redirect("/login");
            ctx.HandleResponse();
            return Task.CompletedTask;
        },

        OnForbidden = ctx =>
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
            ctx.Response.Redirect("/404");
            ctx.Fail("the requested resource could not be found");
            return Task.CompletedTask;
        },

        OnMessageReceived = ctx =>
        {
            ctx.Request.Query.TryGetValue("authorization", out var query);
            ctx.Request.Headers.TryGetValue("authorization", out var header);
            ctx.Request.Cookies.TryGetValue("authorization", out var cookie);
            ctx.Token = (string?)query ?? (string?)header ?? cookie;
            return Task.CompletedTask;
        },
    };

    public static readonly TokenValidationParameters TokenValidationParameters = new()
    {
        ValidIssuer = ClaimsIssuer,
        ValidAudience = ClaimsAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
    };

    private static readonly JwtSecurityTokenHandler Handler = new();
    private static readonly SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes(Key));
    private static SigningCredentials SigningCredentials => new(SecurityKey, SecurityAlgorithms.HmacSha256);

    public static RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var str = Convert.ToBase64String(randomNumber);
        return new(str, DateTime.UtcNow.AddDays(7));
    }

    public static User ValidateRefreshToken(User user, RefreshToken refreshToken)
    {
        if (user.RefreshToken!.Token != refreshToken.Token || user.RefreshToken!.ExpireTime <= DateTime.UtcNow)
        {
            return null!;
        }

        return user;
    }


    public static string GenerateToken(User user)
    {
        var exp = DateTime.UtcNow.AddMinutes(10);
        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email),
            new Claim("role", user.Role.ToString()),
        };
        var token = new JwtSecurityToken(
            ClaimsIssuer,
            ClaimsAudience,
            claims,
            expires: exp,
            signingCredentials: SigningCredentials);

        return Handler.WriteToken(token);
    }
}