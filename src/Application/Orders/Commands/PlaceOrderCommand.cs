using Application.Services;
using Domain.Aggregates;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Orders.Commands;

public sealed record PlaceOrderCommand : IRequest<PlaceOrderResult>
{
    public required User User { get; set; }

    public required Order Order { get; set; }
}

public abstract record PlaceOrderResult
{
    public sealed record Success(Order Order) : PlaceOrderResult;

    public sealed record Failure(IEnumerable<string> Errors) : PlaceOrderResult;
}

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.User)
            .NotNull();

        RuleFor(x => x.Order)
            .NotNull();
    }
}

public sealed record PlaceOrderCommandHandler(IAppDbContext DbContext) : IRequestHandler<PlaceOrderCommand, PlaceOrderResult>
{
    public async Task<PlaceOrderResult> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        request.User.Orders?.Add(request.Order);
        DbContext.Set<User>().Update(request.User);
        await DbContext.SaveChangesAsync(ct);
        return new PlaceOrderResult.Success(request.Order);
    }
}