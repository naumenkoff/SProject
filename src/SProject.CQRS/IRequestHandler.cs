using System.Diagnostics.CodeAnalysis;

namespace SProject.CQRS;

public interface IRequestHandler<TResponse> where TResponse : IResponse
{
    Task<TResponse> ExecuteAsync(IRequest<TResponse> request);
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IRequestHandler<in TRequest, TResponse> : IRequestHandler<TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse
{
    Task<TResponse> ExecuteAsync(TRequest request);
}