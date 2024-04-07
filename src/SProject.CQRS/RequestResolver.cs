using Microsoft.Extensions.DependencyInjection;

namespace SProject.CQRS;

internal sealed class RequestResolver(IServiceProvider serviceProvider) : IRequestResolver
{
    public Task<TResponse> Execute<TResponse>(IRequest<TResponse> request) where TResponse : IResponse
    {
        var targetServiceType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var requestHandler = (IRequestHandler<TResponse>)serviceProvider.GetRequiredService(targetServiceType);
        return requestHandler.Execute(request);
    }
}