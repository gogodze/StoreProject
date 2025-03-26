namespace Domain.Entities;

public sealed record User
{
    public Ulid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string HashedPassword { get; init; } = null!;
    public DateTime RegisterDate { get; init; }
    public string Email { get; init; } = null!;
    public string Address { get; init; } = null!;
    public IEnumerable<Order>? Orders { get; set; }
    public byte[]? ProfilePicture { get; init; }
}