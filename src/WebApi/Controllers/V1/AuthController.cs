using Application.Users.Queries;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] string username, [FromBody] string password)
    {
        var User = await mediator.Send(new GetUserByEmailAndPassword(username, password));
        if (User == null)
            return Unauthorized();
        var token = JwtGenerator.GenerateToken(User, TimeSpan.FromMinutes(5));
        return Ok(token);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}