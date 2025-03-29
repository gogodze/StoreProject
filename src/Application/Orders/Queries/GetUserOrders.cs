using Application.Interfaces;
using Domain.Aggregates;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries;

public sealed record GetUserOrdersCommand(Ulid UserId) : IRequest<IEnumerable<Order>?>;

public sealed record GetUserOrdersHandler(IAppDbContext DbContext)
    : IRequestHandler<GetUserOrdersCommand, IEnumerable<Order>?>
{
    public async Task<IEnumerable<Order>?> Handle(GetUserOrdersCommand request, CancellationToken ct)
    {
        var resp = await DbContext.Set<User>().Include(x => x.Orders).Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync(ct);
        return resp?.Orders;
    }
}