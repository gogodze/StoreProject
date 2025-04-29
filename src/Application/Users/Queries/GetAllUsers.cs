using Application.Services;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public sealed record GetAllUsersQuery(int PageSize, int PageNumber) : IRequest<IEnumerable<User>>;

public sealed record GetAllUsersQueryHandler(IAppDbContext DbContext) : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        return await DbContext.Set<User>().AsNoTracking().OrderBy(x => x.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize).ToListAsync(ct);
    }
}