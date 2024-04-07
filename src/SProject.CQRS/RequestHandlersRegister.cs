using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SProject.CQRS;

public static class RequestHandlersRegister
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IRequestResolver, RequestResolver>();

        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(t => t.IsClass)))
        foreach (var @interface in type.GetInterfaces().Where(x => x.IsGenericType))
        {
            var definition = @interface.GetGenericTypeDefinition();
            if (definition == typeof(IRequestHandler<>) || definition == typeof(IRequestHandler<,>))
                serviceCollection.TryAddTransient(@interface, type);
        }

        return serviceCollection;
    }
}