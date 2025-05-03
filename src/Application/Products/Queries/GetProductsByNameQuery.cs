using Application.Services;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries;

public sealed record GetProductsByNameQuery(string Name, int PageSize, int PageNumber) : IRequest<IEnumerable<Product>>;

public class GetProductsByNameQueryValidator : AbstractValidator<GetProductsByNameQuery>
{
    public GetProductsByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .GreaterThan(0);
    }
}

public sealed record GetProductsByNameQueryHandler(IAppDbContext DbContext)
    : IRequestHandler<GetProductsByNameQuery, IEnumerable<Product>>
{
    public async Task<IEnumerable<Product>> Handle(GetProductsByNameQuery request, CancellationToken ct)
    {
        return await DbContext.Set<Product>().Where(o => o.ProductName == request.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(o => o.ProductName)
            .ToListAsync(ct);
    }
}