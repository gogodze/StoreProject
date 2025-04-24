using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries;

public sealed record GetProductsOnSaleQuery : IRequest<List<Product>>;

public sealed record GetProductsOnSalQueryHandler(IAppDbContext DbContext) : IRequestHandler<GetProductsOnSaleQuery, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsOnSaleQuery request, CancellationToken ct)
    {
        var resp = await DbContext.Set<Product>().Where(x => x.DiscountAmountPercent != 0).ToListAsync(ct);
        return resp;
    }
}