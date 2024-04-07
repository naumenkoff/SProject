namespace SProject.CQRS;

public interface IRequestResolver
{
    Task<TResponse> Execute<TResponse>(IRequest<TResponse> request) where TResponse : IResponse;
}