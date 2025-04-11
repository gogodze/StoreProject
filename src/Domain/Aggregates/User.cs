using System.ComponentModel.DataAnnotations;
using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public sealed class User : AggregateRoot
{
    public Ulid Id { get; init; }

    [MaxLength(50)]
    public string Name { get; init; } = null!;

    [MaxLength(50)]

    public string Surname { get; init; } = null!;

    [MaxLength(30)]
    public string UserName { get; init; } = null!;

    [MaxLength(256)]
    public string HashedPassword { get; init; } = null!;

    public Role Role { get; init; }

    public DateTime RegisterDate { get; init; }

    [MaxLength(100)]
    public string Email { get; init; } = null!;

    public Address? Address { get; init; }
    public ICollection<Order>? Orders { get; set; }

    public byte[]? ProfilePicture { get; init; }
}