namespace Domain.Entities;

public sealed record Product
{
    public int Id { get; init; }
    public string ProductName { get; init; } = null!;
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public int DiscountAmountPercent { get; init; }
    public Category Category { get; init; } = null!;
    public byte[]? PreviewImage { get; init; }
}