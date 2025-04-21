using Domain.ValueObjects;

namespace Domain.Common;

public static class RefreshTokenExt
{
    public static string RefreshTokenToString(this RefreshToken refreshToken)
    {
        return $"{refreshToken.Token}:{refreshToken.ExpireTime}";
    }
}