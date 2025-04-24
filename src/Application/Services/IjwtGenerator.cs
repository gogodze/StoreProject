using Domain.Aggregates;

namespace Application.Services;

public interface IJwtGenerator
{
    public string GenerateToken(User user);
}