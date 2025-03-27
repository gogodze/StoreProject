using Domain.Aggregates;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed record Order
{
    public Ulid Id { get; init; }

    public Ulid UserId { get; init; }
    public User User { get; init; } = null!;

    public required ICollection<Product> Products { get; init; }

    public OrderDetails? Details { get; init; }
}