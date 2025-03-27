namespace Domain.ValueObjects;

public sealed record OrderDetails
{
    public decimal Total { get; init; }
    public DateTime DateOrdered { get; init; }
    public DateTime DateOrderFinished { get; init; }
}