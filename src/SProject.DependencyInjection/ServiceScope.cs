using Microsoft.Extensions.DependencyInjection;
using SProject.DependencyInjection.Abstractions;

namespace SProject.DependencyInjection;

public sealed class ServiceScope<T> : IServiceScope<T> where T : class
{
    private readonly IServiceScope _scope;
    private readonly ServiceTracker<T> _serviceTracker;
    private bool _disposed;

    public ServiceScope(IServiceScope scope)
    {
        _scope = scope;
        _serviceTracker = new ServiceTracker<T>(Dispose);
    }

    public IServiceWrapper<T> GetRequiredService()
    {
        var service = _scope.ServiceProvider.GetRequiredService<T>();
        return _serviceTracker.Add(service);
    }

    public IServiceWrapper<T> GetRequiredKeyedService(object? serviceKey)
    {
        var service = _scope.ServiceProvider.GetRequiredKeyedService<T>(serviceKey);
        return _serviceTracker.Add(service);
    }

    public IServiceWrapper<T?> GetService()
    {
        var service = _scope.ServiceProvider.GetService<T>();
        return _serviceTracker.Add(service);
    }

    public IServiceWrapper<T?> GetKeyedService(object? serviceKey)
    {
        var service = _scope.ServiceProvider.GetKeyedService<T>(serviceKey);
        return _serviceTracker.Add(service);
    }

    public IEnumerable<IServiceWrapper<T>> GetServices()
    {
        return _scope.ServiceProvider.GetServices<T>().Select(x => _serviceTracker.Add(x));
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
            if (disposing) _scope.Dispose();
            _disposed = true;
        }
    }

    ~ServiceScope()
    {
        Dispose(false);
    }

    private readonly struct ServiceTracker<TService>(Action servicesDisposedCallback) where TService : class?
    {
        private readonly List<TService> _services = [];
        private readonly object _lock = new();

        public IServiceWrapper<TService> Add(TService service)
        {
            var serviceWrapper = new AsyncServiceWrapper<TService>(service, DisposeCallback);

            lock (_lock)
            {
                _services.Add(service);
            }

            return serviceWrapper;
        }

        private void DisposeCallback(TService service)
        {
            lock (_lock)
            {
                _services.Remove(service);
                if (_services.Count == 0) servicesDisposedCallback.Invoke();
            }
        }
    }
}