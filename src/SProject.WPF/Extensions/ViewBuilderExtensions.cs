using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SProject.WPF.Abstractions;
using SProject.WPF.HostedServices;

namespace SProject.WPF.Extensions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ViewBuilderExtensions
{
    public static IServiceCollection AddView<TMainView, TMainViewModel>(this IServiceCollection serviceCollection)
        where TMainView : IMainViewOf<TMainViewModel>
        where TMainViewModel : ObservableObject
    {
        return serviceCollection.AddHostedService<StartupService<TMainViewModel>>().RegisterView<TMainView>();
    }

    private static IServiceCollection RegisterView<TView>(this IServiceCollection serviceCollection)
    {
        foreach (var type in typeof(TView).Assembly.GetTypes())
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