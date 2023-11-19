using System.Text.RegularExpressions;
using SProject.FileSystem;

namespace SProject.Steam.Abstractions;

public class SteamClientModel
{
    private readonly ISteamClientTemplateProvider _templateProvider;

    public SteamClientModel(ISteamClientTemplateProvider templateProvider)
    {
        _templateProvider = templateProvider;
    }

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

    public IEnumerable<SteamClientModel> GetAnotherInstallations(ISteamClientTemplateProvider? steamClientTemplateProvider = default,
        bool throwExceptionIfNotFound = false)
    {
        var steamapps = GetSteamappsDirectory(throwExceptionIfNotFound);
        if (steamapps is null) return Enumerable.Empty<SteamClientModel>();

        var libraryfolders = FileSystemInfoExtensions.GetFileInfo(throwExceptionIfNotFound, steamapps.FullName, "libraryfolders.vdf");
        if (libraryfolders is null) return Enumerable.Empty<SteamClientModel>();

        var fileContent = libraryfolders.ReadAllText(throwExceptionIfNotFound);
        if (string.IsNullOrEmpty(fileContent)) return Enumerable.Empty<SteamClientModel>();

        var clients = new List<SteamClientModel>();

        var (selector, template) = _templateProvider.GetTemplate(SteamClientTemplateType.Instance);
        foreach (Match match in template.Matches(fileContent))
        {
            var directoryInfo = FileSystemInfoExtensions.GetDirectoryInfo(false, selector(match));
            if (directoryInfo is null) continue;

            var steamClientModel = new SteamClientModel(steamClientTemplateProvider ?? _templateProvider)
            {
                WorkingDirectory = directoryInfo,
                IsRootDirectory = false
            };

            clients.Add(steamClientModel);
        }

        if (throwExceptionIfNotFound && clients.Count == 0) throw new NotImplementedException();

        return clients;
    }
}