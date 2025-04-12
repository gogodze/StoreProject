using Application.Interfaces;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Commands;

public sealed record PlaceOrderCommand : IRequest<bool>
{
    [LogMasked]
    public required User User { get; set; }

    [LogMasked]
    public required Order Order { get; set; }
}

public sealed record PlaceOrderCommandHandler(IAppDbContext DbContext) : IRequestHandler<PlaceOrderCommand, bool>
{
    public async Task<bool> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        var user = await DbContext.Set<User>().Where(x => x.Id == request.User.Id).FirstOrDefaultAsync(ct);
        if (user is null) return false;
        user.Orders?.Add(request.Order);
        DbContext.Set<User>().Add(user);
        await DbContext.SaveChangesAsync(ct);
        return true;
    }
}