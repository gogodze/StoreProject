using Application.Users.Commands;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand login)
    {
        var user = await mediator.Send(login);
        if (user != null)
        {
            var token = JwtGenerator.GenerateToken(user);
            var refreshToken = JwtGenerator.GenerateRefreshToken();
            var resp = await mediator.Send(new UpdateRefreshTokenCommand
            {
                Userid = user.Id,
                RefreshToken = refreshToken
            });
            return resp ? Ok(new { token, refreshToken }) : BadRequest("error writing refresh token");
        }

        return BadRequest("invalid credentials");
    }


    [HttpPost("register")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand customer)
    {
        var user = await mediator.Send(customer);
        return Ok(user);
    }

    // [HttpPost("refresh-token")]
    // public async Task<IActionResult> RefreshToken([FromBody] RegisterCustomerCommand customer)
    // {
    //     var user = await mediator.Send(customer);
    //     return Ok(user);
    // }
}