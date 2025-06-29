using Application.Services;
using MediatR;
using Serilog;

namespace Application.Behaviours;

internal sealed class RequestLoggingBehaviour<TRequest, TResponse>(ICurrentUserAccessor currentUser) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var userId = currentUser.Id?.ToString() ?? "unauthenticated";
        Log.Information("{UserId} sent request {RequestName} {@Request}", userId, typeof(TRequest).Name, request);

        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "{UserId} failed to handle request {RequestName} {@Request}", userId, typeof(TRequest).Name, request);
            throw;
        }
    }
}