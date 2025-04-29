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
    public required string FullName { get; init; }

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
            FullName = request.FullName,
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