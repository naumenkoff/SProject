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

    public static DirectoryInfo CreateDirectory()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), nameof(TestHelper));
        var directoryInfo = new DirectoryInfo(tempDirectory);
        directoryInfo.Create();
        return directoryInfo;
    }
}