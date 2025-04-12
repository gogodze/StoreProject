using Application.Interfaces;
using Destructurama.Attributed;
using Domain.Aggregates;
using MediatR;

namespace Application.Users.Commands;

public sealed record RegisterUserCommand : IRequest<bool>
{
    [LogMasked]
    public required User User { get; set; }
}

public sealed record RegisterUserCommandHandler(IAppDbContext DbContext) : IRequestHandler<RegisterUserCommand, bool>
{
    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        await DbContext.Set<User>().AddAsync(request.User, ct);
        await DbContext.SaveChangesAsync(ct);
        return true;
    }
}