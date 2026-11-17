using Application.Auth;
using Application.Users.Commands;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace Client.Auth;

internal static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").DisableAntiforgery();

        group.MapPost("/login", LoginAsync);
        group.MapPost("/logout", LogoutAsync);
        group.MapPost("/register", RegisterCustomerAsync);

        return endpoints;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        IMediator mediator,
        Application.Services.IJwtGenerator jwtGenerator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
        };

        var validation = await new LoginCommandValidator().ValidateAsync(command, ct);
        if (!validation.IsValid)
            return Results.BadRequest(new AuthErrorResponse(validation.Errors.Select(e => e.ErrorMessage).ToArray()));

        var user = await mediator.Send(command, ct);
        if (user is null)
            return Results.Unauthorized();

        var refreshToken = RefreshToken.CreateNew();
        await mediator.Send(new UpdateRefreshTokenCommand
        {
            Userid = user.Id,
            RefreshToken = refreshToken,
        }, ct);

        var token = jwtGenerator.GenerateToken(user);
        AuthCookie.Append(httpContext, token);

        return Results.Ok(new AuthSuccessResponse(user.FullName, user.Role.ToString()));
    }

    private static IResult LogoutAsync(HttpContext httpContext)
    {
        AuthCookie.Delete(httpContext);
        return Results.Ok();
    }

    private static async Task<IResult> RegisterCustomerAsync(
        RegisterCustomerRequest request,
        IMediator mediator,
        Application.Services.IJwtGenerator jwtGenerator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var command = new RegisterCustomerCommand
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
        };

        var validation = await new RegisterCustomerCommandValidator().ValidateAsync(command, ct);
        if (!validation.IsValid)
            return Results.BadRequest(new AuthErrorResponse(validation.Errors.Select(e => e.ErrorMessage).ToArray()));

        try
        {
            var user = await mediator.Send(command, ct);

            var refreshToken = RefreshToken.CreateNew();
            await mediator.Send(new UpdateRefreshTokenCommand
            {
                Userid = user.Id,
                RefreshToken = refreshToken,
            }, ct);

            var token = jwtGenerator.GenerateToken(user);
            AuthCookie.Append(httpContext, token);

            return Results.Ok(new AuthSuccessResponse(user.FullName, user.Role.ToString()));
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new AuthErrorResponse([ex.Message]));
        }
    }
}

internal sealed record LoginRequest(string Email, string Password);

internal sealed record RegisterCustomerRequest(
    string Name,
    string Surname,
    string Email,
    string Password,
    string ConfirmPassword);

internal sealed record AuthSuccessResponse(string FullName, string Role);

internal sealed record AuthErrorResponse(string[] Errors);
