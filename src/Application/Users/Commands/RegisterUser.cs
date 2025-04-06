using Application.Interfaces;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands;

public sealed record RegisterUserCommand(string email, string password) : IRequest<User?>;

public sealed record RegisterUserCommandHandler(IAppDbContext DbContext) : IRequestHandler<RegisterUserCommand, User?>
{
    public async Task<User?> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => (x.Email == request.email)
                                                      & (x.HashedPassword == request.password)).FirstOrDefaultAsync(ct);
    }
}