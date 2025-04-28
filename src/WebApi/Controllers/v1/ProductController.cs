using Application.Products.Commands;
using Application.Products.Queries;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[Authorize]
[ApiController]
public class ProductController(IMediator mediator) : ApiController
{
    [AllowAnonymous]
    [HttpGet("getSale")]
    public async Task<IActionResult> GetSale()
    {
        var request = await mediator.Send(new GetProductsOnSaleQuery(10, 1));
        return Ok(request);
    }

    [HttpGet("/products/{name}")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var request = await mediator.Send(new GetProductsByNameQuery(name, 10, 1));
        return Ok(request);
    }

    [Authorize(Roles = nameof(Role.ProductManager))]
    [HttpPost("/products/add")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
    {
        var request = await mediator.Send(command);
        return Ok(request);
    }
}