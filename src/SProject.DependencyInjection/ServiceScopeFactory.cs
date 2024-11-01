using System.Diagnostics.CodeAnalysis;
using SProject.DependencyInjection.Abstractions;
using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;

namespace SProject.DependencyInjection;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public sealed class ServiceScopeFactory<T>(IServiceScopeFactory serviceScopeFactory) : IServiceScopeFactory<T> where T : class
{
    public IServiceScope<T> CreateScope()
    {
        return new ServiceScope<T>(serviceScopeFactory.CreateScope());
    }
}