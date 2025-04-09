using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public sealed class User : AggregateRoot
{
    public Ulid Id { get; init; }
    public string Name { get; init; } = null!;

    public string Surname { get; init; } = null!;

    public string UserName { get; init; } = null!;

    public string HashedPassword { get; init; } = null!;

    public Role Role { get; init; }

    public DateTime RegisterDate { get; init; }
    public string Email { get; init; } = null!;

    public Address? Address { get; init; }
    public ICollection<Order>? Orders { get; set; }

    public byte[]? ProfilePicture { get; init; }
}