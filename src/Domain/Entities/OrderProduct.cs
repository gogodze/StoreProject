namespace Domain.Entities;

public sealed record OrderProduct
{
    public Ulid Id { get; init; }

    public Ulid OrderId { get; init; }
    public Order Order { get; init; } = null!;

    public Ulid ProductId { get; init; }

    public Product Product { get; init; } = null!;
}