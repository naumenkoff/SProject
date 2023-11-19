using System.ComponentModel;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using SProject.FileSystem;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

public sealed class SteamClientFinder : ISteamClientFinder
{
    private readonly ISteamClientTemplateProvider _steamClientTemplateProvider;
    private readonly SteamOptions _steamOptions;

    public SteamClientFinder(IOptions<SteamOptions> steamOptions, ISteamClientTemplateProvider steamClientTemplateProvider)
    {
        _steamOptions = steamOptions.Value;
        _steamClientTemplateProvider = steamClientTemplateProvider;
    }

    public SteamClientModel? FindSteamClient()
    {
        foreach (var node in _steamOptions.SteamPathNodes)
        {
            using var hive = RegistryKey.OpenBaseKey(node.PathHive, RegistryView.Registry64);
            using var steam = hive.OpenSubKey(node.Path);
            if (steam?.GetValue(node.Name) is not string installPath) continue;

            var directoryInfo = FileSystemInfoExtensions.GetDirectoryInfo(false, installPath);
            if (directoryInfo is not null)
            {
                return new SteamClientModel(_steamClientTemplateProvider)
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