using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
public class OrderController(IMediator mediator) : ApiController
{
    [HttpGet("getMyOrders/{userId}")]
    public async Task<IActionResult> GetUsersOrders(Ulid userId)
    {
        await mediator.Send(new GetUserOrdersCommand(userId));
        return Ok();
    }

    [HttpPost("placeOrder")]
    public async Task<IActionResult> PlaceOrder()
    {
        // await mediator.Send(new PlaceOrderCommand());
        return Ok();
    }
}