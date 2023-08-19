namespace SProject;

/// <summary>
///     Provides extension methods for working with <see cref="System.IO.DirectoryInfo" /> instances.
/// </summary>
public static class DirectoryInfoExtensions
{
    public static FileInfo[] GetAllFiles(this DirectoryInfo directoryInfo)
    {
        return directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
    }

    public static FileInfo[] GetAllFiles(this DirectoryInfo directoryInfo, Func<FileInfo, bool> predicate)
    {
        return directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Where(predicate).ToArray();
    }

    public static IEnumerable<FileInfo> EnumerateAllFiles(this DirectoryInfo directoryInfo)
    {
        return directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories);
    }

    public static IEnumerable<FileInfo> EnumerateAllFiles(this DirectoryInfo directoryInfo, Func<FileInfo, bool> predicate)
    {
        return directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(predicate);
    }

    public static TResult[] GetDirectoriesAs<TResult>(this DirectoryInfo directoryInfo, Func<DirectoryInfo, TResult> selector)
    {
        return directoryInfo.GetDirectories().Select(selector).ToArray();
    }

    public static IEnumerable<TResult> EnumerateDirectoriesAs<TResult>(this DirectoryInfo directoryInfo, Func<DirectoryInfo, TResult> selector)
    {
        return directoryInfo.EnumerateDirectories().Select(selector);
    }
}