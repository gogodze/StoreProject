using Application.Services;
using Domain.Aggregates;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetUserByIdQuery : IRequest<User?>
{
    public required Ulid? Id { get; set; }
}

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public sealed record GetUserByIdQueryHandler(IAppDbContext DbContext) : IRequestHandler<GetUserByIdQuery, User?>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        return await DbContext.Set<User>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(ct);
    }
}