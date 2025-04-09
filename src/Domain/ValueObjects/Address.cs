namespace Domain.ValueObjects;

public sealed record Address
{
    public string AddressLine1 { get; init; } = null!;
    public string? AddressLine2 { get; init; }
    public string City { get; init; } = null!;
    public string? State { get; init; }
    public string Country { get; init; } = null!;
    public int ZipCode { get; init; }
}