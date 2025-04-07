using Application.Interfaces;
using Domain.Aggregates;
using MediatR;

namespace Application.Users.Commands;

public sealed record RegisterUserCommand(User user) : IRequest<bool>;

public sealed record RegisterUserCommandHandler(IAppDbContext DbContext) : IRequestHandler<RegisterUserCommand, bool>
{
    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        await DbContext.Set<User>().AddAsync(request.user, ct);
        await DbContext.SaveChangesAsync(ct);
        return true;
    }
}