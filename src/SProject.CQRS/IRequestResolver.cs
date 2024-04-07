namespace SProject.CQRS;

public interface IRequestResolver
{
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request) where TResponse : IResponse;
}