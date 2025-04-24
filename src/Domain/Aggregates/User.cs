using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public sealed class User : AggregateRoot
{
    public Ulid Id { get; init; }

    public RefreshToken? RefreshToken { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }

    public required string Surname { get; set; }


    public required string UserName { get; set; }


    public required string HashedPassword { get; set; }


    public Role Role { get; set; }

    public DateTime RegisterDate { get; init; }


    public Address? Address { get; set; }
    public ICollection<Order>? Orders { get; set; }

    public byte[]? ProfilePicture { get; set; }
}