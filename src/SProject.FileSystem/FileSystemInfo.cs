using System.Diagnostics.CodeAnalysis;

namespace SProject.FileSystem;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class FileSystemInfoExtensions
{
    public static FileInfo GetFile(this FileSystemInfo fileSystemInfo, params string[] paths)
    {
        return (FileInfo)Create<FileInfo>(fileSystemInfo.FullName, paths);
    }

    public static DirectoryInfo GetDirectory(this FileSystemInfo fileSystemInfo, params string[] paths)
    {
        return (DirectoryInfo)Create<DirectoryInfo>(fileSystemInfo.FullName, paths);
    }

    public static FileInfo GetFile(params string[] paths)
    {
        return (FileInfo)Create<FileInfo>(paths);
    }

    public static DirectoryInfo GetDirectory(params string[] paths)
    {
        return (DirectoryInfo)Create<DirectoryInfo>(paths);
    }

    public static bool HasSubdirectory(this DirectoryInfo directoryInfo, string name)
    {
        return directoryInfo.GetDirectory(name).Exists();
    }

    public static bool Exists([NotNullWhen(true)] this FileSystemInfo? fileSystemInfo)
    {
        return fileSystemInfo is { Exists: true };
    }

    public static bool NotExists([NotNullWhen(false)] this FileSystemInfo? fileSystemInfo)
    {
        return fileSystemInfo is not { Exists: true };
    }

    public static FileSystemInfo Create<T>(string[] paths) where T : FileSystemInfo
    {
        var path = Path.Combine(paths);
        return typeof(T) switch
        {
            var type when type == typeof(FileInfo) => (new FileInfo(path) as T)!,
            var type when type == typeof(DirectoryInfo) => (new DirectoryInfo(path) as T)!,
            _ => throw new ArgumentException($"Unsupported FileSystemInfo type: {typeof(T).Name}")
        };
    }

    public static FileSystemInfo Create<T>(string initialPath, string[] source) where T : FileSystemInfo
    {
        var destination = new string[source.Length + 1];
        destination[0] = initialPath;
        source.CopyTo(destination, 1);
        return Create<T>(destination);
    }
}