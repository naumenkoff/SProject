using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SProject.CQRS;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class RequestHandlersRegister
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IRequestResolver, RequestResolver>();

        foreach (var implementationType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(t => t.IsClass))
        foreach (var serviceType in from @interface in implementationType.GetInterfaces()
                                    where @interface.IsGenericType
                                    let genericType = @interface.GetGenericTypeDefinition()
                                    where genericType == typeof(IRequestHandler<>) || genericType == typeof(IRequestHandler<,>)
                                    select @interface)
            serviceCollection.TryAddTransient(serviceType, implementationType);

        return serviceCollection;
    }
}