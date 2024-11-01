using System.Diagnostics.CodeAnalysis;

namespace SProject.Steam;

[ExcludeFromCodeCoverage]
public class InstalledApplication
{
    public int AppId { get; init; }
    public string? Name { get; init; }
    public long LastOwner { get; init; }
    public DateTimeOffset LastPlayed { get; init; }
    public bool FromGameLibrary { get; init; }
    public bool FromAppmanifest { get; init; }
    public DirectoryInfo? InstallDir { get; init; }

    public static InstalledApplication Combine(InstalledApplication x, InstalledApplication y)
    {
        var appmanifest = x.FromAppmanifest ? x : y;
        var gameLibrary = x.FromGameLibrary ? x : y;

        if (gameLibrary.AppId != appmanifest.AppId)
            throw new
                InvalidOperationException($"Cannot combine the specified {nameof(InstalledApplication)} because they represent different instances ({nameof(gameLibrary)}.{nameof(gameLibrary.AppId)} = {gameLibrary.AppId}, but {nameof(appmanifest)}.{nameof(appmanifest.AppId)} = {appmanifest.AppId}).");

        return new InstalledApplication
        {
            FromGameLibrary = gameLibrary.FromGameLibrary || appmanifest.FromGameLibrary,
            FromAppmanifest = gameLibrary.FromAppmanifest || appmanifest.FromAppmanifest,
            Name = appmanifest.Name,
            AppId = appmanifest.AppId,
            LastOwner = appmanifest.LastOwner,
            LastPlayed = appmanifest.LastPlayed,
            InstallDir = appmanifest.InstallDir
        };
    }
}