using Application.Interfaces;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Users.Commands;

public sealed record RegisterCustomerCommand : IRequest<User>
{
    [LogMasked]
    public required string Name { get; init; }

    [LogMasked]
    public required string Surname { get; init; }

    [LogMasked]
    public required string UserName { get; init; }

    [LogMasked]
    public required string HashedPassword { get; init; }

    [LogMasked]
    public required string Email { get; init; }

    [LogMasked]
    public required Address? Address { get; init; }

    [LogMasked]
    public byte[]? ProfilePicture { get; init; } = null;
}

public sealed record RegisterCustomerCommandHandler(IAppDbContext DbContext) : IRequestHandler<RegisterCustomerCommand, User>
{
    public async Task<User> Handle(RegisterCustomerCommand request, CancellationToken ct)
    {
        var user = new User
        {
            Id = Ulid.NewUlid(),
            Name = request.Name,
            Surname = request.Surname,
            UserName = request.UserName,
            HashedPassword = request.HashedPassword,
            Role = Role.Customer,
            RegisterDate = DateTime.Now,
            Email = request.Email,
            Address = request.Address,
            Orders = null,
            ProfilePicture = request.ProfilePicture,
        };
        await DbContext.Set<User>().AddAsync(user, ct);
        await DbContext.SaveChangesAsync(ct);
        return user;
    }
}