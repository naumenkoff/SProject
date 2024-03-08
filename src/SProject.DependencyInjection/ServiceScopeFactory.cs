using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;

namespace SProject.DependencyInjection;

public class ServiceScopeFactory<T>(IServiceScopeFactory serviceScopeFactory) : IServiceScopeFactory<T> where T : class
{
    public IServiceScope<T> CreateScope()
    {
        return new ServiceScope<T>(serviceScopeFactory.CreateScope());
    }
}