namespace Domain.ValueObjects;

public sealed record Address
{
    public required string AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public required string City { get; init; }
    public string? State { get; init; }
    public required string Country { get; init; }
    public required string ZipCode { get; init; }
}