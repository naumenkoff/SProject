namespace SProject.TestHelper;

public static class FileSystemInfoCreator
{
    public static FileInfo CreateFile(DirectoryInfo tempDirectory)
    {
        var tempFile = Path.Combine(tempDirectory.FullName, Path.GetRandomFileName());
        var file = new FileInfo(tempFile);
        file.Create().Dispose();
        return file;
    }

    public static DirectoryInfo CreateDirectory(string name)
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), name);
        var directoryInfo = new DirectoryInfo(tempDirectory);
        directoryInfo.Create();
        return directoryInfo;
    }
}