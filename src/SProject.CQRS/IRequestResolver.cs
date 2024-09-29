using System.Diagnostics.CodeAnalysis;

namespace SProject.CQRS;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IRequestResolver
{
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request) where TResponse : IResponse;
}