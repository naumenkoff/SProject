using System.Diagnostics.CodeAnalysis;

namespace SProject.FileSystem;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static partial class DirectoryInfoExtensions
{
    private const string AllFiles = "*.*";

    public static FileInfo[] GetAllFiles(this DirectoryInfo directoryInfo)
    {
        return directoryInfo.GetFiles(AllFiles, SearchOption.AllDirectories);
    }

    public static IEnumerable<FileInfo> EnumerateAllFiles(this DirectoryInfo directoryInfo)
    {
        return directoryInfo.EnumerateFiles(AllFiles, SearchOption.AllDirectories);
    }

    public static IEnumerable<FileInfo> EnumerateAllFiles(this DirectoryInfo directoryInfo, Func<FileInfo, bool> predicate)
    {
        return directoryInfo.EnumerateAllFiles().Where(predicate);
    }

    public static FileInfo[] GetAllFiles(this DirectoryInfo directoryInfo, Func<FileInfo, bool> predicate)
    {
        return directoryInfo.EnumerateAllFiles(predicate).ToArray();
    }
}