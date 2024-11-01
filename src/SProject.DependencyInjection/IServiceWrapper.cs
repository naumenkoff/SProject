using System.Diagnostics.CodeAnalysis;

namespace SProject.DependencyInjection.Abstractions;

[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface IServiceWrapper<out T> : IAsyncDisposable, IDisposable where T : class?
{
    T Service { get; }
}