using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using Domain.ValueObjects;
using EntityFrameworkCore.DataProtection.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Application.Users.Commands;

public sealed record RegisterCustomerCommand : IRequest<User>
{
    [LogMasked]
    public string FullName { get; set; } = null!;

    [LogMasked]
    public string Password { get; set; } = null!;

    [LogMasked]
    public string Email { get; set; } = null!;

    [LogMasked]
    public string ConfirmPassword { get; set; } = null!;
}

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator(IAppDbContext dbContext)
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(15)
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Full name must contain only letters and spaces.");


        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50);


        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password).WithMessage("Passwords must match.")
            .MinimumLength(6)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(50)
            .WithMessage("Email must be a valid email address and not exceed 50 characters.");

        RuleSet("async",
            () =>
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .MustAsync(async (_, email, ct) =>
                    {
                        var usersWithPd = await dbContext.Set<User>().WherePdEquals(nameof(User.Email), email).CountAsync(ct);
                        return usersWithPd == 0;
                    })
                    .WithMessage("Email already exists."));
    }
}

public sealed record RegisterCustomerCommandHandler(IAppDbContext DbContext) : IRequestHandler<RegisterCustomerCommand, User>
{
    public async Task<User> Handle(RegisterCustomerCommand request, CancellationToken ct)
    {
        var transaction = await DbContext.BeginTransactionAsync(ct);

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
        await transaction.CommitAsync(ct);
        return user;
    }
}