using Microsoft.Extensions.DependencyInjection;

namespace SProject.DependencyInjection;

public sealed class ServiceScope<T>(IServiceScope scope) : IServiceScope<T>
    where T : class
{
    private bool _disposed;

    public T GetRequiredService()
    {
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public T GetRequiredKeyedService(object? serviceKey)
    {
        return scope.ServiceProvider.GetRequiredKeyedService<T>(serviceKey);
    }

    public T? GetService()
    {
        return scope.ServiceProvider.GetService<T>();
    }

    public T? GetKeyedService(object? serviceKey)
    {
        return scope.ServiceProvider.GetKeyedService<T>(serviceKey);
    }

    public IEnumerable<T> GetServices()
    {
        return scope.ServiceProvider.GetServices<T>();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) scope.Dispose();
            _disposed = true;
        }
    }

    ~ServiceScope()
    {
        Dispose(false);
    }
}