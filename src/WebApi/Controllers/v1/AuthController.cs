using Application.Auth;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand login)
    {
        LoginCommandValidator commandValidator = new();
        await commandValidator.ValidateAsync(login);
        var result = await mediator.Send(login);
        return result is LoginResult.Success ? Ok(result) : BadRequest(result);
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