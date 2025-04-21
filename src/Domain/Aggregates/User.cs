using System.ComponentModel.DataAnnotations;
using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public sealed class User : AggregateRoot
{
    public Ulid Id { get; init; }

    public RefreshToken? RefreshToken { get; set; }

    [MaxLength(50)]
    public required string Name { get; init; }

    [MaxLength(50)]

    public required string Surname { get; init; }

    [MaxLength(30)]
    public required string UserName { get; init; }

    [MaxLength(256)]
    public required string HashedPassword { get; init; }

    public Role Role { get; init; }

    public DateTime RegisterDate { get; init; }

    [MaxLength(100)]
    public required string Email { get; init; }

    public Address? Address { get; init; }
    public ICollection<Order>? Orders { get; set; }

    public byte[]? ProfilePicture { get; init; }
}