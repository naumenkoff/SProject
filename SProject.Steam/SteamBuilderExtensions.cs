using Microsoft.Extensions.DependencyInjection;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

public static class SteamClientServiceCollectionExtensions
{
    public static IServiceCollection AddSteamClient(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<ISteamClientFinder, SteamClientFinder>();
        services.AddSingleton<ISteamClientTemplateProvider, SteamClientTemplateProvider>();

        services.AddOptions<SteamOptions>();
        return services;
    }
}