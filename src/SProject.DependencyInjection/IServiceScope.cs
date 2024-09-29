using System.Diagnostics.CodeAnalysis;

namespace SProject.DependencyInjection;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IServiceScope<out T> : IDisposable where T : class
{
    T GetRequiredService();

    T GetRequiredKeyedService(object? serviceKey);

    T? GetService();

    T? GetKeyedService(object? serviceKey);

    IEnumerable<T> GetServices();
}