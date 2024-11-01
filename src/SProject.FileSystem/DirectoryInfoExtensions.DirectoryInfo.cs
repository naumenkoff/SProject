using System.Diagnostics.CodeAnalysis;

namespace SProject.FileSystem;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static partial class DirectoryInfoExtensions
{
    private const string AllDirectories = "*.*";

    public static TResult[] GetDirectoriesAs<TResult>(this DirectoryInfo directoryInfo, Func<DirectoryInfo, TResult> selector)
    {
        return directoryInfo.EnumerateDirectoriesAs(selector).ToArray();
    }

    public static IEnumerable<TResult> EnumerateDirectoriesAs<TResult>(this DirectoryInfo directoryInfo, Func<DirectoryInfo, TResult> selector)
    {
        return directoryInfo.EnumerateDirectories().Select(selector);
    }

    public static DirectoryInfo[] GetAllDirectories(this DirectoryInfo directoryInfo)
    {
        return directoryInfo.GetDirectories(AllDirectories, SearchOption.AllDirectories);
    }

    public static IEnumerable<DirectoryInfo> EnumerateAllDirectories(this DirectoryInfo directoryInfo)
    {
        return directoryInfo.EnumerateDirectories(AllDirectories, SearchOption.AllDirectories);
    }

    public static IEnumerable<DirectoryInfo> EnumerateAllDirectories(this DirectoryInfo directoryInfo, Func<DirectoryInfo, bool> predicate)
    {
        return directoryInfo.EnumerateAllDirectories().Where(predicate);
    }

    public static DirectoryInfo[] GetAllDirectories(this DirectoryInfo directoryInfo, Func<DirectoryInfo, bool> predicate)
    {
        return directoryInfo.EnumerateAllDirectories(predicate).ToArray();
    }
}