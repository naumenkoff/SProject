using System.IO.Enumeration;
using TimeSpan = System.TimeSpan;

namespace SProject.FileSystem.Tests;

[TestFixture]
public class DirectoryInfoExtensionsTest
{
    [OneTimeSetUp]
    public void Init()
    {
        _testDirectory = TestHelper.FileSystemInfoCreator.CreateDirectory();
        _subTestDirectory = _testDirectory.CreateSubdirectory(DateTime.UtcNow.Ticks.ToString());
        _testFileMain = TestHelper.FileSystemInfoCreator.CreateFile(_testDirectory);
        _testFileSecond = TestHelper.FileSystemInfoCreator.CreateFile(_testDirectory);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _testDirectory.Delete(true);
    }

    private DirectoryInfo _testDirectory = null!;
    private DirectoryInfo _subTestDirectory = null!;
    private FileInfo _testFileMain = null!;
    private FileInfo _testFileSecond = null!;

    [Test]
    public void GetAllFiles_ShouldReturnFileInfoArray()
    {
        // Arrange & Act
        var files = _testDirectory.GetAllFiles();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(files.GetType(), Is.EqualTo(typeof(FileInfo[])));
            Assert.That(files.Any(x => x.FullName == _testFileMain.FullName), Is.True);
        });
    }

    [Test]
    public void GetAllFiles_ShouldReturnFilteredFileInfoArray()
    {
        // Arrange & Act
        var files = _testDirectory.GetAllFiles(x => x.FullName == _testFileSecond.FullName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(files.GetType(), Is.EqualTo(typeof(FileInfo[])));
            Assert.That(files.First().FullName, Is.EqualTo(_testFileSecond.FullName));
        });
    }

    [Test]
    public void EnumerateAllFiles_ShouldReturnEnumerable()
    {
        // Arrange & Act
        var files = _testDirectory.EnumerateAllFiles();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(files.GetType(), Is.EqualTo(typeof(FileSystemEnumerable<FileInfo>)));
            Assert.That(files.Any(x => x.FullName == _testFileMain.FullName), Is.True);
        });
    }

    [Test]
    public void EnumerateAllFiles_ShouldReturnFilteredEnumerable()
    {
        // Arrange & Act
        var files = _testDirectory.EnumerateAllFiles(x => x.FullName == _testFileSecond.FullName);

        // Assert
        Assert.That(files.First().FullName, Is.EqualTo(_testFileSecond.FullName));
    }

    [Test]
    public void GetDirectoriesAs_ShouldReturnTimeSpanArray()
    {
        // Arrange & Act
        var directories = _testDirectory.GetDirectoriesAs(x => TimeSpan.FromTicks(long.Parse(x.Name)));

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(directories.GetType(), Is.EqualTo(typeof(TimeSpan[])));
            Assert.That(directories.First().Ticks.ToString(), Is.EqualTo(_subTestDirectory.Name));
        });
    }

    [Test]
    public void EnumerateDirectoriesAs_ShouldReturnTimeSpanEnumerable()
    {
        // Arrange & Act
        var directories = _testDirectory.EnumerateDirectoriesAs(x => TimeSpan.FromTicks(long.Parse(x.Name)));

        // Assert
        Assert.That(directories.First().Ticks.ToString(), Is.EqualTo(_subTestDirectory.Name));
    }
}