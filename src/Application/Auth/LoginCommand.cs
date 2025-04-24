using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;


namespace Application.Auth;

public sealed record LoginCommand : IRequest<LoginResult>
{
    [LogMasked]
    public required string Email { get; set; }


    [LogMasked]
    public required string Password { get; set; }
}

public abstract record LoginResult
{
    public sealed record Success(User User, string Token) : LoginResult;

    public sealed record Failure(IEnumerable<string> Errors) : LoginResult;
}

public sealed record LoginCommandHandler(IAppDbContext DbContext, IJwtGenerator JwtGenerator) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await DbContext.Set<User>().Where(x => x.Email == request.Email).FirstOrDefaultAsync(ct);
        if (user == null || !EnhancedVerify(request.Password, user.HashedPassword))
            return new LoginResult.Failure(["Invalid email or password"]);
        var token = JwtGenerator.GenerateToken(user);
        user.RefreshToken = RefreshToken.CreateNew();
        DbContext.Set<User>().Update(user);
        return new LoginResult.Success(user, token);
    }
}