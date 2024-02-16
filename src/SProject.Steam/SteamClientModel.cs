using SProject.FileSystem;
using SProject.VDF;

namespace SProject.Steam;

public class SteamClientModel
{
    private IEnumerable<SteamClientModel>? _steamClientModels;
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
        if (force)
        {
            // Force - we want to get a fresh list of Steam libraries.
            // As a result, the method is not thread-safe.
            _steamClientModels = null;
        }
        else
        {
            // Otherwise, if we have already been called at least once,
            // we can return the cached result.
            if (_steamClientModels is not null) return _steamClientModels;
        }

        var steamapps = GetSteamappsDirectory(throwExceptionIfNotFound);
        if (steamapps is null) return _steamClientModels = Enumerable.Empty<SteamClientModel>();

        var libraryfolders = FileSystemInfoExtensions.GetFileInfo(throwExceptionIfNotFound, steamapps.FullName, "libraryfolders.vdf");
        if (libraryfolders is null) return _steamClientModels = Enumerable.Empty<SteamClientModel>();

        var rootObject = ByteVdfParser.Parse(libraryfolders);
        if (!rootObject.HasValues) return _steamClientModels = Enumerable.Empty<SteamClientModel>();

        // There may be ghost directories here,
        // so throwing an exception, by 'throw exception if not found' parameter value,
        // will lead to inconsistent behavior.
        var clients = rootObject.AllObjects.Enumerate("path").Select(path => FileSystemInfoExtensions.GetDirectoryInfo(false, path))
            .OfType<DirectoryInfo>().Select(directory => new SteamClientModel
            {
                WorkingDirectory = directory,
                IsRootDirectory = IsRootDirectory && directory.FullName == WorkingDirectory.FullName
            }).ToList();

        if (throwExceptionIfNotFound && clients.Count == 0) throw new NotImplementedException();
        return _steamClientModels = clients;
    }
}