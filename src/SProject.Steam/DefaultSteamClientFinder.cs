using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

[SupportedOSPlatform("windows")]
public sealed class DefaultSteamClientFinder(
    IOptions<SteamOptions> steamOptions,
    ISteamInstallPathResolver<SteamPathNode> steamInstallPathResolver,
    ILogger<DefaultSteamClientFinder> logger) : ISteamClientFinder
{
    public IEnumerable<SteamClientModel> FindSteamClients()
    {
        foreach (var steamPathNode in steamOptions.Value.SteamPathNodes)
        {
            var installPath = steamInstallPathResolver.Resolve(steamPathNode);
            if (string.IsNullOrEmpty(installPath))
            {
                logger.LogTrace("SteamNode[{Hive}][{Node}][{Name}]: Unresolved[{Path}]",
                                steamPathNode.PathHive, steamPathNode.Path, steamPathNode.Name, installPath);
                continue;
            }

            var directoryInfo = new DirectoryInfo(installPath);
            if (!directoryInfo.Exists)
            {
                logger.LogTrace("SteamNode[{Hive}][{Node}][{Name}]: Fake[{Path}]",
                                steamPathNode.PathHive, steamPathNode.Path, steamPathNode.Name, directoryInfo.FullName);
                continue;
            }

            yield return new SteamClientModel(directoryInfo, true);
        }
    }
}