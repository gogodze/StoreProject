using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Domain.Aggregates;
using Domain.ValueObjects;

namespace Domain.Common;

public static class ClaimsPrincipalExt
{
    public const string IdClaimType = "sid";
    public const string UsernameClaimType = "name";
    public const string EmailClaimType = "email";
    public const string RoleClaimType = "role";

    public static Ulid? GetId(this ClaimsPrincipal principal) => Ulid.TryParse(
        principal.Claims.FirstOrDefault(c => c.Type == IdClaimType)?.Value, null, out var id)
        ? id
        : null;

    public static string? GetUsername(this ClaimsPrincipal principal) => principal.Claims.FirstOrDefault(c => c.Type == UsernameClaimType)?.Value;
    public static string? GetEmail(this ClaimsPrincipal principal) => principal.Claims.FirstOrDefault(c => c.Type == EmailClaimType)?.Value;

    public static Role? GetRole(this ClaimsPrincipal principal) =>
        Enum.TryParse<Role>(principal.Claims.FirstOrDefault(c => c.Type == RoleClaimType)?.Value, out var role)
            ? role
            : null;

    public static IEnumerable<Claim> GetAllClaims(this User user) =>
    [
        new(IdClaimType, user.Id.UlidToString()),
        new(UsernameClaimType, user.FullName),
        new(EmailClaimType, user.Email),
        new(RoleClaimType, user.Role.ToString()),
    ];


    public static string GetDefaultAvatar(string? username = null)
    {
        username ??= RandomNumberGenerator.GetHexString(5);
        username = UrlEncoder.Default.Encode(username);
        return $"https://api.dicebear.com/9.x/glass/svg?backgroundType=gradientLinear&scale=50&seed={username}";
    }
}