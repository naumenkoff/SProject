using Microsoft.Win32;

namespace SProject.Steam;

public class SteamPathNode
{
    /// <summary>
    ///     Path to Valve/Steam registry key
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    ///     Name of value that contains steam path
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     A hive where Path go
    /// </summary>
    public required RegistryHive PathHive { get; init; }
}