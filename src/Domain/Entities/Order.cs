namespace Domain.Entities;

public sealed record Order
{
    public int Id { get; init; }
    public User User { get; init; } = null!;
    public DateTime DateOrdered { get; init; }
    public DateTime DateOrderFinished { get; init; }
    public List<Product> Product { get; init; } = null!;
}