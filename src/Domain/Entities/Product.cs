using Domain.ValueObjects;

namespace Domain.Entities;

public sealed record Product
{
    public Ulid Id { get; init; }
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int DiscountAmountPercent { get; set; }
    public ProductCategory Category { get; set; }
    public byte[]? PreviewImage { get; set; }
}