using Microsoft.Win32;

namespace SProject.Steam;

internal class DefaultSteamInstallPathResolver : ISteamInstallPathResolver<SteamPathNode>
{
    public string? GetInstallPath(SteamPathNode node)
    {
        using var hive = RegistryKey.OpenBaseKey(node.PathHive, RegistryView.Registry64);
        using var steam = hive.OpenSubKey(node.Path);
        return steam?.GetValue(node.Name) as string;
    }
}