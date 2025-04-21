namespace Domain.ValueObjects;

public sealed record RefreshToken(string Token, DateTime? ExpireTime);