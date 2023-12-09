using SProject.Steam.Abstractions;

namespace SProject.Steam.Tests;

[TestFixture]
public class SteamClientModelTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        _directoryInfo = new DirectoryInfo("fake");
        _steamClientModel = new SteamClientModel
        {
            IsRootDirectory = true,
            WorkingDirectory = _directoryInfo
        };
    }

    private SteamClientModel _steamClientModel = null!;
    private DirectoryInfo _directoryInfo = null!;

    [Test]
    public void CreatingSteamClient_SetsRequiredMembers_WithObjectInitializer()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(_steamClientModel.WorkingDirectory, Is.EqualTo(_directoryInfo));
            Assert.That(_steamClientModel.IsRootDirectory, Is.True);
        });
    }

    [Test]
    public void GetUserdataDirectory_ReturnsNull_WhenWorkingDirectoryIsNull()
    {
        // Act & Assert
        Assert.That(_steamClientModel.GetUserdataDirectory(), Is.Null);
    }

    [Test]
    public void GetConfigDirectory_ReturnsNull_WhenWorkingDirectoryIsNull()
    {
        // Act & Assert
        Assert.That(_steamClientModel.GetConfigDirectory(), Is.Null);
    }

    [Test]
    public void GetSteamappsDirectory_ReturnsNull_WhenWorkingDirectoryIsNull()
    {
        // Act & Assert
        Assert.That(_steamClientModel.GetSteamappsDirectory(), Is.Null);
    }
}