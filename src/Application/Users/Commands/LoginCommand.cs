using Application.Interfaces;
using Destructurama.Attributed;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;


namespace Application.Users.Commands;

public sealed record LoginCommand : IRequest<User?>
{
    [LogMasked]
    public required string Email { get; set; }


    [LogMasked]
    public required string Password { get; set; }
}

public sealed record LoginCommandHandler(IAppDbContext DbContext) : IRequestHandler<LoginCommand, User?>
{
    public async Task<User?> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await DbContext.Set<User>().Where(x => x.Email == request.Email).FirstOrDefaultAsync(ct);
        if (user == null || !EnhancedVerify(request.Password, user.HashedPassword))
            return null;
        return user;
    }
}