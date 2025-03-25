namespace Domain.Entities;

public sealed record User
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string HashedPassword { get; init; } = null!;
    public DateTime RegisterDate { get; init; }
    public string Email { get; init; } = null!;
    public string Address { get; init; } = null!;
    public List<Order>? Orders { get; init; }
    public byte[]? ProfilePicture { get; init; }
}