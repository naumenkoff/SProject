using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace SProject.Steam;

[ExcludeFromCodeCoverage]
[SupportedOSPlatform("windows")]
public class SteamOptions
{
    public List<SteamPathNode> SteamPathNodes { get; init; } = [];

    public static SteamOptions Default => new()
    {
        SteamPathNodes =
        [
            new SteamPathNode
            {
                Name = "SteamPath",
                Path = @"Software\Valve\Steam",
                PathHive = RegistryHive.CurrentUser
            },

            new SteamPathNode
            {
                Name = "InstallPath",
                Path = @"SOFTWARE\WOW6432Node\Valve\Steam",
                PathHive = RegistryHive.LocalMachine
            }
        ]
    };
}