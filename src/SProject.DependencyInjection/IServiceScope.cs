using System.Diagnostics.CodeAnalysis;

namespace SProject.DependencyInjection.Abstractions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IServiceScope<out T> : IDisposable where T : class?
{
    IServiceWrapper<T> GetRequiredService();

    IServiceWrapper<T> GetRequiredKeyedService(object? serviceKey);

    IServiceWrapper<T?> GetService();

    IServiceWrapper<T?> GetKeyedService(object? serviceKey);

    IEnumerable<IServiceWrapper<T>> GetServices();
}