using Domain.Entities;

namespace Domain.ValueObjects;

public sealed record OrderDetails
{
    public DateTime DateOrdered { get; init; }
    public DateTime DateOrderFinished { get; init; }
}