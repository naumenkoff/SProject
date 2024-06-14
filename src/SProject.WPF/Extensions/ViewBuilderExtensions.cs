using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SProject.WPF.Abstractions;
using SProject.WPF.HostedServices;

namespace SProject.WPF.Extensions;

public static class ViewBuilderExtensions
{
    public static IServiceCollection AddView<T, TVm>(this IServiceCollection serviceCollection)
        where T : IMainViewOf<TVm> where TVm : ObservableObject
    {
        return serviceCollection.AddHostedService<StartupService<T, TVm>>().RegisterView<T>();
    }

    private static IServiceCollection RegisterView<T>(this IServiceCollection serviceCollection)
    {
        foreach (var type in typeof(T).Assembly.GetTypes())
        {
            if (!type.IsClass)
                continue;

            if (type.IsAbstract)
                continue;

            var interfaces = type.GetInterfaces();
            foreach (var inter in interfaces)
            {
                if (!inter.IsGenericType)
                    continue;

                var serviceType = inter.GetGenericTypeDefinition();
                if (serviceType == typeof(IMainViewOf<>)) Inject(inter, type);
                else if (serviceType == typeof(IViewOf<>)) Inject(inter, type);
            }
        }

        return serviceCollection;

        void Inject(Type serviceType, Type implementationType)
        {
            var viewModelType = serviceType.GenericTypeArguments.Single();
            serviceCollection.TryAddSingleton(viewModelType);
            serviceCollection.TryAddSingleton(serviceType, implementationType);
        }
    }
}