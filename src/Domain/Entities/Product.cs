using Domain.ValueObjects;

namespace Domain.Entities;

public sealed record Product
{
    public Ulid Id { get; init; }
    public string ProductName { get; init; } = null!;
    public string ProductDescription { get; init; } = null!;
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public int DiscountAmountPercent { get; init; }
    public ProductCategory Category { get; init; }
    public byte[]? PreviewImage { get; init; }
}