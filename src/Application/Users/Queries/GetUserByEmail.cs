using Application.Interfaces;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetUserByEmail(string Email) : IRequest<User?>;

public sealed record GetUserByEmailHandler(IAppDbContext DbContext) : IRequestHandler<GetUserByEmail, User?>
{
    public async Task<User?> Handle(GetUserByEmail request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => x.Email == request.Email).FirstOrDefaultAsync(ct);
    }
}