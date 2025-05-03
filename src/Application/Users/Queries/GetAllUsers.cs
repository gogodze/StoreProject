using Application.Services;
using Domain.Aggregates;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetAllUsersQuery(int PageSize, int PageNumber) : IRequest<IEnumerable<User>>;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .GreaterThan(0);
    }
}

public sealed record GetAllUsersQueryHandler(IAppDbContext DbContext) : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        return await DbContext.Set<User>().AsNoTracking().OrderBy(x => x.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToListAsync(ct);
    }
}