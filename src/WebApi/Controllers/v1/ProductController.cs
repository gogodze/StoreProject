using Application.Products.Queries;
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
        var request = await mediator.Send(new GetProductsOnSaleQuery());
        return Ok(request);
    }

    [HttpGet("/products/{name}")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var request = await mediator.Send(new GetProductsByNameQuery(name));
        return Ok(request);
    }
}