using Application.Interfaces;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetUserByEmailAndPassword(string email, string password) : IRequest<User?>;

public sealed record GetUserByEmailAndPasswordHandler(IAppDbContext DbContext) : IRequestHandler<GetUserByEmailAndPassword, User?>
{
    public async Task<User?> Handle(GetUserByEmailAndPassword request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => (x.Email == request.email)
                                                      & (x.HashedPassword == request.password)).FirstOrDefaultAsync(ct);
    }
}