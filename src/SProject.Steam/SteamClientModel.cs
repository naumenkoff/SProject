using System.Collections.Immutable;
using SProject.FileSystem;
using SProject.VDF;
using SProject.VDF.Extensions;

namespace SProject.Steam;

public class SteamClientModel
{
    public SteamClientModel(DirectoryInfo workingDirectory, bool isRootDirectory)
    {
        WorkingDirectory = workingDirectory;
        IsRootDirectory = isRootDirectory;
        InstalledApplications = GetInstalledApplications().ToImmutableList();
    }

    private SteamClientModel(DirectoryInfo workingDirectory, bool isRootDirectory, IEnumerable<InstalledApplication> gameLibraryApplications)
    {
        WorkingDirectory = workingDirectory;
        IsRootDirectory = isRootDirectory;
        InstalledApplications = CombineInstalledApplications(gameLibraryApplications);
    }

    public ImmutableList<InstalledApplication> InstalledApplications { get; }
    public bool IsRootDirectory { get; }
    public DirectoryInfo WorkingDirectory { get; }

    private ImmutableList<InstalledApplication> CombineInstalledApplications(IEnumerable<InstalledApplication> gameLibraryApplications)
    {
        var installedApplications = new List<InstalledApplication>();
        foreach (var installedApplication in gameLibraryApplications.Concat(GetInstalledApplications()))
        {
            var index = installedApplications.FindIndex(x => x.AppId == installedApplication.AppId);
            if (index == -1) installedApplications.Add(installedApplication);
            else installedApplications[index] = InstalledApplication.Combine(installedApplication, installedApplications[index]);
        }

        return installedApplications.ToImmutableList();
    }

    public DirectoryInfo GetUserdataDirectory()
    {
        return WorkingDirectory.GetDirectory("userdata");
    }

    public DirectoryInfo GetConfigDirectory()
    {
        return WorkingDirectory.GetDirectory("config");
    }

    public DirectoryInfo GetSteamappsDirectory()
    {
        return WorkingDirectory.GetDirectory("steamapps");
    }

    public IEnumerable<InstalledApplication> GetInstalledApplications()
    {
        var steamapps = GetSteamappsDirectory();
        if (steamapps.NotExists()) yield break;

        foreach (var appmanifest in steamapps.EnumerateFiles("appmanifest_*.acf", SearchOption.TopDirectoryOnly))
        {
            var valveDataDocument = ValveDataFileParser.Parse(appmanifest);
            if (!valveDataDocument.HasProperties) yield break;

            var appState = valveDataDocument.PrimarySection.Properties;
            var installDir = appState["installdir"]?.Value;
            yield return new InstalledApplication
            {
                AppId = appState["appid"].AsInt32().GetValueOrDefault(),
                LastPlayed = appState["LastPlayed"].AsDateTimeOffset().GetValueOrDefault(),
                Name = appState["name"]?.Value,
                LastOwner = appState["LastOwner"].AsInt64().GetValueOrDefault(),
                FromGameLibrary = false,
                FromAppmanifest = true,
                InstallDir = string.IsNullOrEmpty(installDir) ? null : steamapps.GetDirectory("common", installDir)
            };
        }
    }

    private static IEnumerable<InstalledApplication> GetInstalledApplications(ValveDataSection? gameLibrarySection)
    {
        var appsSection = gameLibrarySection?["apps"];
        if (appsSection is null) yield break;

        foreach (var appProperty in appsSection.Properties)
        {
            if (!int.TryParse(appProperty.Key, out var appId)) continue;
            yield return new InstalledApplication
            {
                AppId = appId,
                FromGameLibrary = true
            };
        }
    }

    public IEnumerable<SteamClientModel> GetSteamLibraries()
    {
        var steamapps = GetSteamappsDirectory();
        if (steamapps.NotExists()) yield break;

        var libraryfolders = steamapps.GetFile("libraryfolders.vdf");
        if (libraryfolders.NotExists()) yield break;

        var valveDataDocument = ValveDataFileParser.Parse(libraryfolders);
        if (!valveDataDocument.HasProperties) yield break;

        foreach (var gameLibrarySection in valveDataDocument.PrimarySection)
        {
            var pathProperty = gameLibrarySection.Properties["path"];
            if (string.IsNullOrEmpty(pathProperty?.Value)) continue;

            var steamLibrary = FileSystemInfoExtensions.GetDirectory(pathProperty.Value);
            if (steamLibrary.NotExists()) continue;

            if (steamLibrary.HasSubdirectory("steamapps"))
            {
                var installedApplications = GetInstalledApplications(gameLibrarySection);
                var isRootDirectory = IsRootDirectory &&
                                      string.Equals(steamLibrary.FullName, WorkingDirectory.FullName, StringComparison.OrdinalIgnoreCase);
                yield return new SteamClientModel(steamLibrary, isRootDirectory, installedApplications);
            }
            else
            {
                Console.WriteLine($"Steam Directory, but don't have steamapps subdirectory: {steamLibrary.FullName}");
            }
        }
    }
}