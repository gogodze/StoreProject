using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using static BCrypt.Net.BCrypt;

namespace Application.Users.Commands;

public sealed record RegisterCustomerCommand : IRequest<User>
{
    [LogMasked]
    public string FullName { get; set; }

    [LogMasked]
    public string Password { get; set; }

    [LogMasked]
    public string Email { get; set; }

    [LogMasked]
    public string ConfirmPassword { get; set; }
}

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(15);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(50);
    }
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