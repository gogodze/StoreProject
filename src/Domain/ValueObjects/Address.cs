namespace Domain.ValueObjects;

public sealed record Address
{
    public string AddressLine1 { get; init; } = null!;
    public string AddressLine2 { get; init; } = null!;
    public string City { get; init; } = null!;
    public string State { get; init; } = null!;
    public string Country { get; init; } = null!;
    public int ZipCode { get; init; }
}