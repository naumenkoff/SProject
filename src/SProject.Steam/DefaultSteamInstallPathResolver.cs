using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

[SupportedOSPlatform("windows")]
internal class DefaultSteamInstallPathResolver(ILogger<DefaultSteamInstallPathResolver> logger) : ISteamInstallPathResolver<SteamPathNode>
{
    [SuppressMessage("ReSharper", "ConditionalAccessQualifierIsNonNullableAccordingToAPIContract")]
    public string? Resolve(SteamPathNode node)
    {
        try
        {
            using var hive = RegistryKey.OpenBaseKey(node.PathHive, RegistryView.Registry64);
            using var steam = hive.OpenSubKey(node.Path);
            return steam?.GetValue(node.Name) as string;
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "SteamNode[{Hive}][{Node}][{Name}]: Failed", node?.PathHive, node?.Path, node?.Name);
            return null;
        }
    }
}