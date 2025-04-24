using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries;

public sealed record GetProductsByNameQuery(string Name) : IRequest<List<Product>>;

public sealed record GetProductsByNameQueryHandler(IAppDbContext DbContext)
    : IRequestHandler<GetProductsByNameQuery, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsByNameQuery request, CancellationToken ct)
    {
        var resp = await DbContext.Set<Product>().Where(o => o.ProductName == request.Name).OrderBy(o => o.Price)
            .ToListAsync(ct);
        return resp;
    }
}