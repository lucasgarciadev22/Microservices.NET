using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviour;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            string requestName = typeof(TRequest).Name;

            _logger.LogError(
                e,
                "Unhandled Exception Occurred with Request Name: {RequestName}, {Request}",
                requestName,
                request
            );
            throw;
        }
    }
}
