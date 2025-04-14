using Application.Interfaces;
using Destructurama.Attributed;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetUserByIdQuery : IRequest<User?>
{
    [LogMasked]
    public required Ulid? Id { get; set; }
}

public sealed record GetUserByIdQueryHandler(IAppDbContext DbContext) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(ct);
    }
}