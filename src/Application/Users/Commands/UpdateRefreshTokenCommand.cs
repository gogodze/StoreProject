using Application.Services;
using Domain.Aggregates;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands;

public sealed record UpdateRefreshTokenCommand : IRequest<bool>
{
    public Ulid Userid { get; set; }

    public RefreshToken RefreshToken { get; set; } = null!;
}

public sealed record UpdateRefreshTokenCommandHandler(IAppDbContext DbContext) : IRequestHandler<UpdateRefreshTokenCommand, bool>
{
    public async Task<bool> Handle(UpdateRefreshTokenCommand request, CancellationToken ct)
    {
        var user = await DbContext.Set<User>().Where(x => x.Id == request.Userid).FirstOrDefaultAsync(ct);
        if (user == null) return false;
        user.RefreshToken = request.RefreshToken;
        DbContext.Set<User>().Update(user);
        await DbContext.SaveChangesAsync(ct);
        return true;
    }
}