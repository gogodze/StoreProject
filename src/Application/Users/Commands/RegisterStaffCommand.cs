using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Application.Users.Commands;

public sealed record RegisterStaffCommand : IRequest<User>
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

    public Role Role { get; set; }

    [LogMasked]
    public string? AddressLine1 { get; set; }

    [LogMasked]
    public string? AddressLine2 { get; set; }

    [LogMasked]
    public string? City { get; set; }

    [LogMasked]
    public string? State { get; set; }

    [LogMasked]
    public string? Country { get; set; }

    [LogMasked]
    public string? ZipCode { get; set; }
}

public class RegisterStaffCommandValidator : AbstractValidator<RegisterStaffCommand>
{
    public RegisterStaffCommandValidator()
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

        RuleFor(x => x.Role)
            .Must(role => role is Role.ProductManager or Role.Support or Role.Administrator)
            .WithMessage("Staff role must be ProductManager, Support, or Administrator.");

        RuleFor(x => x.AddressLine1)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressLine1));

        RuleFor(x => x.AddressLine2)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.AddressLine2));

        RuleFor(x => x.City)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.City));

        RuleFor(x => x.State)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.State));

        RuleFor(x => x.Country)
            .MaximumLength(56)
            .When(x => !string.IsNullOrWhiteSpace(x.Country));

        RuleFor(x => x.ZipCode)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.ZipCode));
    }
}

public sealed record RegisterStaffCommandHandler(IAppDbContext DbContext)
    : IRequestHandler<RegisterStaffCommand, User>
{
    public async Task<User> Handle(RegisterStaffCommand request, CancellationToken ct)
    {
        var emailExists = await DbContext.Set<User>()
            .AnyAsync(x => x.Email == request.Email, ct);
        if (emailExists)
            throw new InvalidOperationException("An account with this email already exists.");

        Address? address = null;
        if (!string.IsNullOrWhiteSpace(request.AddressLine1)
            && !string.IsNullOrWhiteSpace(request.City)
            && !string.IsNullOrWhiteSpace(request.Country)
            && !string.IsNullOrWhiteSpace(request.ZipCode))
        {
            address = new Address
            {
                AddressLine1 = request.AddressLine1.Trim(),
                AddressLine2 = string.IsNullOrWhiteSpace(request.AddressLine2)
                    ? null
                    : request.AddressLine2.Trim(),
                City = request.City.Trim(),
                State = string.IsNullOrWhiteSpace(request.State) ? null : request.State.Trim(),
                Country = request.Country.Trim(),
                ZipCode = request.ZipCode.Trim(),
            };
        }

        var user = new User
        {
            Id = Ulid.NewUlid(),
            FullName = $"{request.Name.Trim()} {request.Surname.Trim()}",
            HashedPassword = EnhancedHashPassword(request.Password),
            Role = request.Role,
            RegisterDate = DateTime.UtcNow,
            Email = request.Email.Trim(),
            Address = address,
        };

        await DbContext.Set<User>().AddAsync(user, ct);
        await DbContext.SaveChangesAsync(ct);
        return user;
    }
}
