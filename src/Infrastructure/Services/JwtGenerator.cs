using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Application.Services;
using Domain.Aggregates;
using Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtGenerator : IJwtGenerator
{
    public const string CookieName = "authorization";
    public static readonly TimeSpan TokenLifetime = TimeSpan.FromDays(7);

    private static readonly string ClaimsIssuer = "JWT__ISSUER".GetFromEnvRequired();
    private static readonly string ClaimsAudience = "JWT__AUDIENCE".GetFromEnvRequired();
    private static readonly string Key = "JWT__KEY".GetFromEnvRequired();

    public static readonly JwtBearerEvents Events = new()
    {
        OnChallenge = ctx =>
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.Found;
            ctx.Response.Redirect("/Login");
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
            ctx.Request.Query.TryGetValue(CookieName, out var query);
            ctx.Request.Headers.TryGetValue(CookieName, out var header);
            ctx.Request.Cookies.TryGetValue(CookieName, out var cookie);
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
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name,
    };

    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly SymmetricSecurityKey _securityKey = new(Encoding.UTF8.GetBytes(Key));
    private SigningCredentials SigningCredentials => new(_securityKey, SecurityAlgorithms.HmacSha256);

    public string GenerateToken(User user)
    {
        var exp = DateTime.UtcNow.Add(TokenLifetime);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
        };
        var token = new JwtSecurityToken(
            ClaimsIssuer,
            ClaimsAudience,
            claims,
            expires: exp,
            signingCredentials: SigningCredentials);

        return _handler.WriteToken(token);
    }
}
