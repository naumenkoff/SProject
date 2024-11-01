using System.Diagnostics.CodeAnalysis;

namespace SProject.CQRS;

public interface IRequestHandler<TResponse> where TResponse : IResponse
{
    Task<TResponse> ExecuteAsync<TRequest>(TRequest request) where TRequest : IRequest<TResponse>;
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IRequestHandler<in TRequest, TResponse> : IRequestHandler<TResponse> where TResponse : IResponse where TRequest : IRequest<TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request);
}