using Application.Interfaces;
using Destructurama.Attributed;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetUserById : IRequest<User?>
{
    [LogMasked]
    public required Ulid? Id { get; set; }
}

public sealed record GetProductsHandler(IAppDbContext DbContext) : IRequestHandler<GetUserById, User?>
{
    public async Task<User?> Handle(GetUserById request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(ct);
    }
}