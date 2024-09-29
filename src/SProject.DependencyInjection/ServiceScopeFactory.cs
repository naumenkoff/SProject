using System.Diagnostics.CodeAnalysis;
using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;

namespace SProject.DependencyInjection;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ServiceScopeFactory<T>(IServiceScopeFactory serviceScopeFactory) : IServiceScopeFactory<T> where T : class
{
    public IServiceScope<T> CreateScope()
    {
        return new ServiceScope<T>(serviceScopeFactory.CreateScope());
    }
}