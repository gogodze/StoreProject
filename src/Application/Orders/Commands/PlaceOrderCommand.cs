using Application.Services;
using Domain.Aggregates;
using Domain.Entities;
using MediatR;

namespace Application.Orders.Commands;

public sealed record PlaceOrderCommand : IRequest<bool>
{
    public required User User { get; set; }

    public required Order Order { get; set; }
}

public sealed record PlaceOrderCommandHandler(IAppDbContext DbContext) : IRequestHandler<PlaceOrderCommand, bool>
{
    public async Task<bool> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        request.User.Orders?.Add(request.Order);
        DbContext.Set<User>().Update(request.User);
        await DbContext.SaveChangesAsync(ct);
        return true;
    }
}