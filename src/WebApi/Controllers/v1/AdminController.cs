using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Authorize(Roles = nameof(Role.Administrator))]
public class AdminController(IMediator mediator) : ApiController
{
    [HttpGet("/admin/users/")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok("ww");
    }
}