using Infrastructure.Services;

namespace Client.Auth;

internal static class AuthCookie
{
    public static void Append(HttpContext httpContext, string token)
    {
        httpContext.Response.Cookies.Append(JwtGenerator.CookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = httpContext.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.Add(JwtGenerator.TokenLifetime),
            IsEssential = true,
        });
    }

    public static void Delete(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(JwtGenerator.CookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = httpContext.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            IsEssential = true,
        });
    }
}
