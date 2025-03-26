namespace Domain.ValueObjects;

public class Address
{
    public string AddressLine1 { get; init; } = null!;
    public string AddressLine2 { get; init; } = null!;
    public int ZipCode { get; init; }
    
}