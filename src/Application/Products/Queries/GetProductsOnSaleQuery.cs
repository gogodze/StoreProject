using Application.Services;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries;

public sealed record GetProductsOnSaleQuery(int PageSize, int PageNumber) : IRequest<List<Product>>;

public class GetProductsOnSaleQueryValidator : AbstractValidator<GetProductsOnSaleQuery>
{
    public GetProductsOnSaleQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .GreaterThan(0);
    }
}

public sealed record GetProductsOnSalQueryHandler(IAppDbContext DbContext) : IRequestHandler<GetProductsOnSaleQuery, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsOnSaleQuery request, CancellationToken ct)
    {
        var resp = await DbContext.Set<Product>().AsNoTracking()
            .Where(x => x.DiscountAmountPercent != 0).Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToListAsync(ct);
        return resp;
    }
}