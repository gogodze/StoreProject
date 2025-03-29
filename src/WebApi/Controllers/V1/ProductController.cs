using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
public class ProductController(IMediator mediator) : ApiController
{
    [HttpGet("getSale")]
    public async Task<IActionResult> GetSale()
    {
        var request = await mediator.Send(new GetProductsOnSale());
        return Ok(request);
    }

    [HttpGet("/products/{name}")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var request = await mediator.Send(new GetProductsByName(name));
        return Ok(request);
    }
}