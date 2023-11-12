namespace SProject.FileSystem;

/// <summary>
///     Provides utility methods for creating instances of <see cref="System.IO.FileSystemInfo" /> types.
/// </summary>
public static class FileSystemInfoExtensions
{
    /// <summary>
    ///     Gets a <see cref="System.IO.FileInfo" /> instance for the specified path.
    /// </summary>
    /// <param name="throwException">Specifies whether to throw an exception on error. If false, returns null on error.</param>
    /// <param name="paths">The paths to combine into a single path.</param>
    /// <returns>
    ///     A <see cref="System.IO.FileInfo" /> instance
    ///     or
    ///     null if not found or <paramref name="throwException" /> is false.
    /// </returns>
    public static FileInfo? GetFileInfo(bool throwException, params string?[] paths)
    {
        var fileSystemInfo = Create<FileInfo>(throwException, paths);
        return fileSystemInfo.Exists() ? (FileInfo) fileSystemInfo! : default;
    }

    /// <summary>
    ///     Gets a <see cref="System.IO.DirectoryInfo" /> instance for the specified path.
    /// </summary>
    /// <param name="throwException">Specifies whether to throw an exception on error. If false, returns null on error.</param>
    /// <param name="paths">The paths to combine into a single path.</param>
    /// <returns>
    ///     A <see cref="System.IO.DirectoryInfo" /> instance
    ///     or
    ///     null if not found or <paramref name="throwException" /> is false.
    /// </returns>
    public static DirectoryInfo? GetDirectoryInfo(bool throwException, params string?[] paths)
    {
        var fileSystemInfo = Create<DirectoryInfo>(throwException, paths);
        return fileSystemInfo.Exists() ? (DirectoryInfo) fileSystemInfo! : default;
    }

    /// <summary>
    ///     Creates a <see cref="System.IO.FileSystemInfo" /> instance for the specified path and type.
    /// </summary>
    /// <param name="throwException">Specifies whether to throw an exception on error. If false, returns null on error.</param>
    /// <param name="paths">The paths to combine into a single path.</param>
    /// <typeparam name="T">The type of <see cref="System.IO.FileSystemInfo" /> to create.</typeparam>
    /// <returns>
    ///     A <see cref="System.IO.FileSystemInfo" /> instance
    ///     or
    ///     null if not found or <paramref name="throwException" /> is false.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if an unsupported <typeparamref name="T" /> is provided.</exception>
    private static FileSystemInfo? Create<T>(bool throwException, params string?[] paths) where T : FileSystemInfo
    {
        string path;
        try
        {
            // Path.Combine checks for null in the whole collection and each element.
            path = Path.Combine(paths!);
        }
        catch (Exception)
        {
            if (throwException) throw;
            return default;
        }

        if (typeof(T) == typeof(FileInfo)) return new FileInfo(path);
        if (typeof(T) == typeof(DirectoryInfo)) return new DirectoryInfo(path);
        throw new ArgumentException("Unsupported FileSystemInfo type", typeof(T).Name);
    }

    /// <summary>
    ///     Checks if the <see cref="System.IO.FileSystemInfo" /> instance exists.
    /// </summary>
    /// <param name="fileSystemInfo">The <see cref="System.IO.FileSystemInfo" /> instance to check.</param>
    /// <returns>True if the instance is not null and exists, otherwise false.</returns>
    public static bool Exists(this FileSystemInfo? fileSystemInfo)
    {
        return fileSystemInfo is { Exists: true };
    }
}