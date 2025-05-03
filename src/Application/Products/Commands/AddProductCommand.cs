using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace Application.Products.Commands;

public sealed record AddProductCommand : IRequest<Product>
{
    public required string ProductName { get; init; }
    public required string ProductDescription { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required int DiscountAmountPercent { get; init; }
    public required ProductCategory Category { get; init; }
    public required byte[]? PreviewImage { get; init; }
};

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.ProductDescription)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(500);

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.DiscountAmountPercent)
            .NotEmpty()
            .InclusiveBetween(0, 100);
    }
}

public sealed record AddProductCommandHandler(IAppDbContext DbContext)
    : IRequestHandler<AddProductCommand, Product>
{
    public async Task<Product> Handle(AddProductCommand request, CancellationToken ct)
    {
        var product = new Product()
        {
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            Price = request.Price,
            Quantity = request.Quantity,
            DiscountAmountPercent = request.DiscountAmountPercent,
            Category = request.Category,
            PreviewImage = request.PreviewImage,
        };
        var resp = await DbContext.Set<Product>().AddAsync(product, ct);
        await DbContext.SaveChangesAsync(ct);
        return product;
    }
}