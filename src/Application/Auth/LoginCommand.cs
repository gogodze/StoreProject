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
    public string Email { get; set; }


    [LogMasked]
    public string Password { get; set; }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(20);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50);
    }
}

public sealed record LoginCommandHandler(IAppDbContext DbContext, IJwtGenerator JwtGenerator) : IRequestHandler<LoginCommand, User?>
{
    public async Task<User?> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await DbContext.Set<User>().Where(x => x.Email == request.Email).FirstOrDefaultAsync(ct);
        if (user == null || !EnhancedVerify(request.Password, user.HashedPassword))
            return null;
        return user;
    }
}