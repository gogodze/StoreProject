using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[Authorize]
[ApiController]
public class OrderController(IMediator mediator) : ApiController
{
    [HttpGet("orders/{userId}")]
    public async Task<IActionResult> GetUsersOrders(Ulid userId)
    {
        var orders = await mediator.Send(new GetUserOrdersQuery
        {
            UserId = userId,
        });
        return Ok(orders);
    }

    [HttpPost("order")]
    public async Task<IActionResult> PlaceOrder()
    {
        // await mediator.Send(new PlaceOrderCommand());
        return Ok();
    }
}