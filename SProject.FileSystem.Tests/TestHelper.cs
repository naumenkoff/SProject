using System.Runtime.CompilerServices;

namespace SProject.FileSystem.Tests;

public static class TestHelper
{
    public static FileInfo CreateFile(DirectoryInfo tempDirectory)
    {
        var tempFile = Path.Combine(tempDirectory.FullName, Path.GetRandomFileName());
        var file = new FileInfo(tempFile);
        using var _ = file.Create();
        return file;
    }

    public static DirectoryInfo CreateDirectory([CallerMemberName] string name = nameof(TestHelper))
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), name);
        var directoryInfo = new DirectoryInfo(tempDirectory);
        directoryInfo.Create();
        return directoryInfo;
    }
}