using System.Diagnostics.CodeAnalysis;

namespace SProject.CQRS;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface IRequest<TResponse> where TResponse : IResponse;