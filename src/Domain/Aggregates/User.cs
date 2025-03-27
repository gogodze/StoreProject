using System.ComponentModel.DataAnnotations;
using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public sealed class User : AggregateRoot
{
    public Ulid Id { get; init; }

    [StringLength(10)]
    public string Name { get; init; } = null!;

    [StringLength(10)]
    public string Surname { get; init; } = null!;

    [StringLength(10)]
    public string UserName { get; init; } = null!;

    [StringLength(50)]
    public string HashedPassword { get; init; } = null!;

    public Role Role { get; init; }

    public DateTime RegisterDate { get; init; }

    [StringLength(15)]
    public string Email { get; init; } = null!;

    public Address Address { get; init; } = null!;
    public ICollection<Order>? Orders { get; set; }

    public byte[]? ProfilePicture { get; init; }
}