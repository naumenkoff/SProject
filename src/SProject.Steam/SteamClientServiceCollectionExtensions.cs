using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

public static class SteamClientServiceCollectionExtensions
{
    [SupportedOSPlatform("windows")]
    public static IServiceCollection AddSteamClient(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddTransient<ISteamClientFinder, DefaultSteamClientFinder>();
        services.TryAddTransient<ISteamInstallPathResolver<SteamPathNode>, DefaultSteamInstallPathResolver>();
        services.AddOptions<SteamOptions>();

        return services;
    }
}