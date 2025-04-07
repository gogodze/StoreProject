using Application.Dtos;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Aggregates;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static BCrypt.Net.BCrypt;

namespace WebApi.Controllers.v1;

[ApiController]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var user = await mediator.Send(new GetUserByEmail(login.Email));
        if (user == null || !Verify(login.Password, user.HashedPassword))
            return Unauthorized("invalid credentials");
        var token = JwtGenerator.GenerateToken(user, TimeSpan.FromMinutes(5));
        return Ok(token);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        return await mediator.Send(new RegisterUserCommand(user)) ? Ok("user registered successfully") : BadRequest("User not registered");
    }
}