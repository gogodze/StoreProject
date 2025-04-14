using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[Authorize]
[ApiController]
public class OrderController(IMediator mediator) : ApiController
{
    [HttpGet("getMyOrders/{userId}")]
    public async Task<IActionResult> GetUsersOrders(Ulid userId)
    {
        await mediator.Send(new GetUserOrdersQuery
        {
            UserId = userId,
        });
        return Ok();
    }

    [HttpPost("placeOrder")]
    public async Task<IActionResult> PlaceOrder()
    {
        // await mediator.Send(new PlaceOrderCommand());
        return Ok();
    }
}