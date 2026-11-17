using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Application.Users.Commands;

public sealed record RegisterCustomerCommand : IRequest<User>
{
    [LogMasked]
    public string Name { get; set; } = string.Empty;

    [LogMasked]
    public string Surname { get; set; } = string.Empty;

    [LogMasked]
    public string Password { get; set; } = string.Empty;

    [LogMasked]
    public string Email { get; set; } = string.Empty;

    [LogMasked]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(40);

        RuleFor(x => x.Surname)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(40);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
    }
}

public sealed record RegisterCustomerCommandHandler(IAppDbContext DbContext)
    : IRequestHandler<RegisterCustomerCommand, User>
{
    public async Task<User> Handle(RegisterCustomerCommand request, CancellationToken ct)
    {
        var emailExists = await DbContext.Set<User>()
            .AnyAsync(x => x.Email == request.Email, ct);
        if (emailExists)
            throw new InvalidOperationException("An account with this email already exists.");

        var user = new User
        {
            Id = Ulid.NewUlid(),
            FullName = $"{request.Name.Trim()} {request.Surname.Trim()}",
            HashedPassword = EnhancedHashPassword(request.Password),
            Role = Role.Customer,
            RegisterDate = DateTime.UtcNow,
            Email = request.Email.Trim(),
        };

        await DbContext.Set<User>().AddAsync(user, ct);
        await DbContext.SaveChangesAsync(ct);
        return user;
    }
}
