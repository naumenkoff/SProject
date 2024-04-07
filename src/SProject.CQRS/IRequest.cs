namespace SProject.CQRS;

public interface IRequest<TResponse> where TResponse : IResponse;