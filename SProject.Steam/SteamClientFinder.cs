using System.ComponentModel;
using Microsoft.Extensions.Options;
using SProject.FileSystem;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

public sealed class SteamClientFinder : ISteamClientFinder
{
    private readonly ISteamInstallPathResolver<SteamPathNode> _steamInstallPathResolver;
    private readonly SteamOptions _steamOptions;

    public SteamClientFinder(IOptions<SteamOptions> steamOptions, ISteamInstallPathResolver<SteamPathNode> steamInstallPathResolver)
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
            {
                return new SteamClientModel
                {
                    WorkingDirectory = directoryInfo,
                    IsRootDirectory = true
                };
            }
        }

        return _steamOptions.AbsenceSteamClientBehavior switch
        {
            AbsenceSteamClientBehavior.Ignore => default,
            AbsenceSteamClientBehavior.Throw => throw new SteamClientNotFoundException("Steam Client directory not found"),
            _ => throw new InvalidEnumArgumentException(nameof(_steamOptions.AbsenceSteamClientBehavior))
        };
    }
}