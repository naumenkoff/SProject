using SProject.FileSystem;
using SProject.VDF;

namespace SProject.Steam.Abstractions;

public class SteamClientModel
{
    private List<SteamClientModel> _steamClientModels = new List<SteamClientModel>();
    public required bool IsRootDirectory { get; init; }
    public required DirectoryInfo WorkingDirectory { get; init; }

    public DirectoryInfo? GetUserdataDirectory(bool throwExceptionIfNotFound = false)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(throwExceptionIfNotFound, WorkingDirectory.FullName, "userdata");
    }

    public DirectoryInfo? GetConfigDirectory(bool throwExceptionIfNotFound = false)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(throwExceptionIfNotFound, WorkingDirectory.FullName, "config");
    }

    public DirectoryInfo? GetSteamappsDirectory(bool throwExceptionIfNotFound = false)
    {
        return FileSystemInfoExtensions.GetDirectoryInfo(throwExceptionIfNotFound, WorkingDirectory.FullName, "steamapps");
    }

    public IEnumerable<SteamClientModel> GetSteamLibraries(bool throwExceptionIfNotFound = false, bool force = false)
    {
        if (!force && _steamClientModels.Count != 0) return _steamClientModels;

        var steamapps = GetSteamappsDirectory(throwExceptionIfNotFound);
        if (steamapps is null) return Enumerable.Empty<SteamClientModel>();

        var libraryfolders = FileSystemInfoExtensions.GetFileInfo(throwExceptionIfNotFound, steamapps.FullName, "libraryfolders.vdf");
        if (libraryfolders is null) return Enumerable.Empty<SteamClientModel>();

        var rootObject = ByteVdfParser.Parse(libraryfolders);
        if (!rootObject.HasValues) return Enumerable.Empty<SteamClientModel>();

        var clients = new List<SteamClientModel>();
        foreach (var vdfValue in rootObject.AllObjects.Enumerate("path"))
        {
            var directoryInfo = FileSystemInfoExtensions.GetDirectoryInfo(false, vdfValue);
            if (directoryInfo is null) continue;

            var steamClientModel = new SteamClientModel
            {
                WorkingDirectory = directoryInfo,
                IsRootDirectory = false
            };

            clients.Add(steamClientModel);
        }

        if (throwExceptionIfNotFound && clients.Count == 0) throw new NotImplementedException();
        _steamClientModels = clients;

        return clients;
    }
}