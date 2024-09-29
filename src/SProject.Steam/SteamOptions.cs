using Microsoft.Win32;

namespace SProject.Steam;

public class SteamOptions
{
    public List<SteamPathNode> SteamPathNodes { get; init; } =
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
    ];

    public bool ThrowOnAbsence { get; init; }
}