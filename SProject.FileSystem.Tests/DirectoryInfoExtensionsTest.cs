using System.IO.Enumeration;
using System.Security.AccessControl;
using TimeSpan = System.TimeSpan;

namespace SProject.FileSystem.Tests;

[TestFixture]
public class DirectoryInfoExtensionsTest
{
    private DirectoryInfo TestDirectory { get; set; }
    private DirectoryInfo SubTestDirectory { get; set; }
    private FileInfo TestFileMain { get; set; }
    private FileInfo TestFileSecond { get; set; }

    private FileInfo CreateFile(string tempDirectory)
    {
        var tempFile = Path.Combine(tempDirectory, Path.GetRandomFileName());
        var file = new FileInfo(tempFile);
        using var _ = file.Create();
        return file;
    }

    [OneTimeSetUp]
    public void Init()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), nameof(DirectoryInfoExtensionsTest));
        TestDirectory = new DirectoryInfo(tempDirectory);
        TestDirectory.Create();
        SubTestDirectory = TestDirectory.CreateSubdirectory(DateTime.UtcNow.Ticks.ToString());
        TestFileMain = CreateFile(tempDirectory);
        TestFileSecond = CreateFile(tempDirectory);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        TestDirectory.Delete(true);
    }

    [Test]
    public void GetAllFiles()
    {
        var files = TestDirectory.GetAllFiles();
        Assert.Multiple(() =>
        {
            Assert.That(files.GetType(), Is.EqualTo(typeof(FileInfo[])));
            Assert.That(files.Any(x => x.FullName == TestFileMain.FullName), Is.True);
        });
    }

    [Test]
    public void GetAllFilesWithPredicate()
    {
        var files = TestDirectory.GetAllFiles(x => x.FullName == TestFileSecond.FullName);
        Assert.Multiple(() =>
        {
            Assert.That(files.GetType(), Is.EqualTo(typeof(FileInfo[])));
            Assert.That(files.First().FullName, Is.EqualTo(TestFileSecond.FullName));
        });
    }

    [Test]
    public void EnumerateAllFiles()
    {
        var files = TestDirectory.EnumerateAllFiles();
        Assert.Multiple(() =>
        {
            Assert.That(files.GetType(), Is.EqualTo(typeof(FileSystemEnumerable<FileInfo>)));
            Assert.That(files.Any(x => x.FullName == TestFileMain.FullName), Is.True);
        });
    }

    [Test]
    public void EnumerateAllFilesPredicate()
    {
        var files = TestDirectory.EnumerateAllFiles(x => x.FullName == TestFileSecond.FullName);
        Assert.That(files.First().FullName, Is.EqualTo(TestFileSecond.FullName));
    }

    [Test]
    public void GetDirectoriesAs()
    {
        var directories = TestDirectory.GetDirectoriesAs(x => TimeSpan.FromTicks(long.Parse(x.Name)));
        Assert.Multiple(() =>
        {
            Assert.That(directories.GetType(), Is.EqualTo(typeof(TimeSpan[])));
            Assert.That(directories.First().Ticks.ToString(), Is.EqualTo(SubTestDirectory.Name));
        });
    }
    
    [Test]
    public void EnumerateDirectoriesAs()
    {
        var directories = TestDirectory.EnumerateDirectoriesAs(x => TimeSpan.FromTicks(long.Parse(x.Name)));
        Assert.That(directories.First().Ticks.ToString(), Is.EqualTo(SubTestDirectory.Name));
    }
}