using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SProject.WPF.Abstractions;
using SProject.WPF.HostedServices;
using SProject.WPF.Services;

namespace SProject.WPF.Extensions;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ViewBuilderExtensions
{
    public static IServiceCollection AddView<T, TVm>(this IServiceCollection serviceCollection)
        where T : IMainViewOf<TVm> where TVm : ObservableObject
    {
        return serviceCollection.AddHostedService<StartupService<TVm>>().RegisterView<T>().AddSingleton<ShutdownService>();
    }

    private static IServiceCollection RegisterView<T>(this IServiceCollection serviceCollection)
    {
        foreach (var implementationType in typeof(T).Assembly.GetTypes().Where(type => type is { IsClass: true, IsAbstract: false }))
        foreach (var serviceType in from @interface in implementationType.GetInterfaces()
                                    where @interface.IsGenericType
                                    let genericType = @interface.GetGenericTypeDefinition()
                                    where genericType == typeof(IMainViewOf<>) || genericType == typeof(IViewOf<>)
                                    select @interface)
            Inject(serviceType, implementationType);

        return serviceCollection;

        void Inject(Type serviceType, Type implementationType)
        {
            var viewModelType = serviceType.GenericTypeArguments.Single();
            serviceCollection.TryAddSingleton(viewModelType);
            serviceCollection.TryAddSingleton(serviceType, implementationType);
        }
    }
}