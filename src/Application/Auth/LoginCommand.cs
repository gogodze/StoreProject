using Application.Services;
using Destructurama.Attributed;
using Domain.Aggregates;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Application.Auth;

public sealed record LoginCommand : IRequest<User?>
{
    [LogMasked]
    public string Email { get; set; } = string.Empty;

    [LogMasked]
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50);
    }
}

public sealed record LoginCommandHandler(IAppDbContext DbContext) : IRequestHandler<LoginCommand, User?>
{
    public async Task<User?> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await DbContext.Set<User>()
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(ct);

        if (user is null || !EnhancedVerify(request.Password, user.HashedPassword))
            return null;

        return user;
    }
}
