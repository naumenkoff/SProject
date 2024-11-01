using SProject.DependencyInjection.Abstractions;

namespace SProject.DependencyInjection;

public readonly struct AsyncServiceWrapper<T> : IServiceWrapper<T> where T : class?
{
    private readonly Action<T> _disposeCallback;

    internal AsyncServiceWrapper(T service, Action<T> disposeCallback)
    {
        _disposeCallback = disposeCallback;
        Service = service;
    }

    public T Service { get; }

    public ValueTask DisposeAsync()
    {
        return DisposeAsync(Service, _disposeCallback);
    }

    private static async ValueTask DisposeAsync<TService>(TService service, Action<TService> serviceDisposed)
    {
        switch (service)
        {
            case IAsyncDisposable asyncDisposable:
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                break;
            }
            case IDisposable disposable:
            {
                disposable.Dispose();
                break;
            }
        }

        serviceDisposed(service);
    }

    public void Dispose()
    {
        Dispose(Service, _disposeCallback);
    }

    private static void Dispose<TService>(TService service, Action<TService> serviceDisposed)
    {
        switch (service)
        {
            case IDisposable disposable:
            {
                disposable.Dispose();
                break;
            }
            case IAsyncDisposable:
            {
                serviceDisposed(service);
                // To avoid deadlocks or other situations, we throw an exception
                throw new NotSupportedException($"For asynchronous disposal, use the {nameof(DisposeAsync)} method instead.");
            }
        }

        serviceDisposed(service);
    }
}