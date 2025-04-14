using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Aggregates;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Authorize]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand login)
    {
        var user = await mediator.Send(login);
        return user != null ? Ok(JwtGenerator.GenerateToken(user, TimeSpan.FromHours(24))) : BadRequest("Invalid credentials");
    }


    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand customer)
    {
        var user = await mediator.Send(customer);
        return Ok(user);
    }

    // public async Task<IActionResult> AdminRegister([FromBody] RegisterCustomerCommand user)
    // {
    //     return await mediator.Send(new RegisterCustomerCommand
    //     {
    //         User = user,
    //     })
    //         ? Ok("user registered successfully")
    //         : BadRequest("User not registered");
    // }
}