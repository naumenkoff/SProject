using Microsoft.Win32;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

public class RegistrySteamInstallPathResolver : ISteamInstallPathResolver<SteamPathNode>
{
    public string? GetInstallPath(SteamPathNode node)
    {
        using var hive = RegistryKey.OpenBaseKey(node.PathHive, RegistryView.Registry64);
        using var steam = hive.OpenSubKey(node.Path);
        return steam?.GetValue(node.Name) as string;
    }
}