using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Aggregates;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand login)
    {
        var user = await mediator.Send(new LoginCommand
        {
            Email = login.Email,
            Password = login.Password,
        });
        return user != null ? Ok(JwtGenerator.GenerateToken(user, TimeSpan.FromHours(24))) : BadRequest("Invalid credentials");
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        return await mediator.Send(new RegisterUserCommand
        {
            User = user,
        })
            ? Ok("user registered successfully")
            : BadRequest("User not registered");
    }
}