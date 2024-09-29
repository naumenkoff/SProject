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

        foreach (var someClass in AppDomain.CurrentDomain
                                      .GetAssemblies()
                                      .SelectMany(x => x.GetTypes())
                                      .Where(t => t.IsClass))
        foreach (var someClassInterface in someClass.GetInterfaces().Where(x => x.IsGenericType))
        {
            var definition = someClassInterface.GetGenericTypeDefinition();
            if (definition == typeof(IRequestHandler<>) || definition == typeof(IRequestHandler<,>))
                serviceCollection.TryAddTransient(someClassInterface, someClass);
        }

        return serviceCollection;
    }
}