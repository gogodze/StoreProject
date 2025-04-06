using Application.Interfaces;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetUserById(Ulid Id) : IRequest<User?>;

public sealed record GetProductsHandler(IAppDbContext DbContext) : IRequestHandler<GetUserById, User?>
{
    public async Task<User?> Handle(GetUserById request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(ct);
    }
}