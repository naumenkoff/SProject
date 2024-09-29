using Microsoft.Extensions.Options;
using SProject.FileSystem;

namespace SProject.Steam;

public sealed class DefaultSteamClientFinder : ISteamClientFinder
{
    private readonly ISteamInstallPathResolver<SteamPathNode> _steamInstallPathResolver;
    private readonly SteamOptions _steamOptions;

    public DefaultSteamClientFinder(IOptions<SteamOptions> steamOptions,
                                    ISteamInstallPathResolver<SteamPathNode> steamInstallPathResolver)
    {
        _steamInstallPathResolver = steamInstallPathResolver;
        _steamOptions = steamOptions.Value;
    }

    public SteamClientModel? FindSteamClient()
    {
        foreach (var node in _steamOptions.SteamPathNodes)
        {
            var installPath = _steamInstallPathResolver.GetInstallPath(node);
            var directoryInfo = FileSystemInfoExtensions.GetDirectoryInfo(false, installPath);
            if (directoryInfo is not null)
                return new SteamClientModel
                {
                    WorkingDirectory = directoryInfo,
                    IsRootDirectory = true
                };
        }

        return _steamOptions.ThrowOnAbsence
            ? throw new DirectoryNotFoundException("Steam Client directory not found")
            : null;
    }
}