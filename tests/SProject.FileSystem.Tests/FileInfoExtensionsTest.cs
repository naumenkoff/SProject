using SProject.TestHelper;

namespace SProject.FileSystem.Tests;

[TestFixture]
public class FileInfoExtensionsTest
{
    [OneTimeSetUp]
    public void Init()
    {
        _testDirectory = FileSystemInfoCreator.CreateDirectory();
        _testFile = FileSystemInfoCreator.CreateFile(_testDirectory);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _testFile.Delete();
        _testDirectory.Delete(true);
    }

    private DirectoryInfo _testDirectory = null!;
    private FileInfo _testFile = null!;

    [Test]
    public void ReadAllText_ThrowsArgumentNullException_WhenFileInfoIsNull()
    {
        // Arrange
        FileInfo? nullFileInfo = null;

        // Act & Assert
        Assert.Catch<ArgumentNullException>(() => nullFileInfo.ReadAllText(true));
    }

    [Test]
    public void ReadAllText_ReturnsNull_WhenFileInfoIsNull()
    {
        // Arrange
        FileInfo? nullFileInfo = null;

        // Act
        var text = nullFileInfo.ReadAllText();

        // Assert
        Assert.That(text, Is.Null);
    }

    [Test]
    public void ReadAllText_ThrowsArgumentNullException_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFile = new FileInfo(nameof(FileInfoExtensionsTest));

        // Act & Assert
        Assert.Catch<ArgumentNullException>(() => nonExistentFile.ReadAllText(true));
    }

    [Test]
    public void ReadAllText_ReturnsNull_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFile = new FileInfo(nameof(FileInfoExtensionsTest));

        // Act
        var text = nonExistentFile.ReadAllText();

        // Assert
        Assert.That(text, Is.Null);
    }

    [Test]
    public void ReadAllText_ReturnsCorrectContent_WhenFileExists()
    {
        // Arrange
        var text = "Hello, world!";
        File.WriteAllText(_testFile.FullName, text);

        // Act
        var content = _testFile.ReadAllText();

        // Assert
        Assert.That(content, Is.EqualTo(text));
    }

    [Test]
    public void ReadAllText_ThrowsIOException_WhenReadingFails()
    {
        // Arrange
        var stream = _testFile.Open(FileMode.Open);

        // Act & Assert
        Assert.Catch<IOException>(() => _testFile.ReadAllText(true));

        // Cleanup
        stream.Dispose();
    }

    [Test]
    public void ReadAllText_ReturnsNull_WhenReadingFails()
    {
        // Arrange
        var stream = _testFile.Open(FileMode.Open);

        // Act
        var text = _testFile.ReadAllText();

        // Assert
        Assert.That(text, Is.Null);

        // Cleanup
        stream.Dispose();
    }
}