using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SProject.Steam;

public static class SteamClientServiceCollectionExtensions
{
    public static IServiceCollection AddSteamClient(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddTransient<ISteamClientFinder, DefaultSteamClientFinder>();
        services.TryAddTransient<ISteamInstallPathResolver<SteamPathNode>, DefaultSteamInstallPathResolver>();
        services.AddOptions<SteamOptions>();

        return services;
    }
}