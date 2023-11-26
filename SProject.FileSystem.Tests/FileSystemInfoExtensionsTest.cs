namespace SProject.FileSystem.Tests;

[TestFixture]
public class FileSystemInfoExtensionsTest
{
    [OneTimeSetUp]
    public void Init()
    {
        _directory = TestHelper.CreateDirectory();
        _file = TestHelper.CreateFile(_directory);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _file.Delete();
        _directory.Delete(true);
    }

    private FileInfo _file = null!;
    private DirectoryInfo _directory = null!;

    [Test]
    public void GetFileInfo_ReturnsFileInfo_WhenFileExists()
    {
        // Act
        var file = FileSystemInfoExtensions.GetFileInfo(false, _file.FullName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(file, Is.Not.Null);
            Assert.That(file?.FullName, Is.EqualTo(_file.FullName));
        });
    }

    [Test]
    public void GetFileInfo_ReturnsNull_WhenFileNotExists()
    {
        // Act
        var file = FileSystemInfoExtensions.GetFileInfo(false, Path.GetRandomFileName());

        // Assert
        Assert.That(file, Is.Null);
    }

    [Test]
    public void GetDirectoryInfo_ReturnsDirectoryInfo_WhenDirectoryExists()
    {
        // Act
        var directory = FileSystemInfoExtensions.GetDirectoryInfo(false, _directory.FullName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(directory, Is.Not.Null);
            Assert.That(directory?.FullName, Is.EqualTo(_directory.FullName));
        });
    }

    [Test]
    public void GetDirectoryInfo_ReturnsNull_WhenDirectoryNotExists()
    {
        // Act
        var directory = FileSystemInfoExtensions.GetDirectoryInfo(false, Path.GetRandomFileName());

        // Assert
        Assert.That(directory, Is.Null);
    }

    [Test]
    public void Create_ReturnsNull_WhenInvalidPath()
    {
        // Act
        var fileSystemInfo = FileSystemInfoExtensions.Create<FileInfo>(false, string.Empty);

        // Assert
        Assert.That(fileSystemInfo, Is.Null);
    }

    [Test]
    public void Create_ThrowsException_WhenInvalidPath()
    {
        // Assert & Act
        Assert.Catch<ArgumentException>(() => FileSystemInfoExtensions.Create<FileInfo>(true, string.Empty));
    }

    [Test]
    public void Create_ReturnsNull_WhenCreatingNotSupportedType()
    {
        // Act
        var fileSystemInfo = FileSystemInfoExtensions.Create<FileSystemInfo>(false, Path.GetRandomFileName());

        // Assert
        Assert.That(fileSystemInfo, Is.Null);
    }

    [Test]
    public void Create_ThrowsException_WhenCreatingNotSupportedType()
    {
        // Assert & Act
        Assert.Catch<ArgumentException>(() => FileSystemInfoExtensions.Create<FileSystemInfo>(true, Path.GetRandomFileName()));
    }
}