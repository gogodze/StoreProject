using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.ValueObjects;
using MediatR;
using static BCrypt.Net.BCrypt;

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
    public required string Password { get; init; }

    [LogMasked]
    public required string Email { get; init; }
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
            HashedPassword = EnhancedHashPassword(request.Password),
            Role = Role.Customer,
            RegisterDate = DateTime.Now,
            Email = request.Email,
        };
        await DbContext.Set<User>().AddAsync(user, ct);
        await DbContext.SaveChangesAsync(ct);
        return user;
    }
}