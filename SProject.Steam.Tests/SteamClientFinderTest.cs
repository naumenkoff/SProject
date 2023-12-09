using System.ComponentModel;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Moq;
using SProject.Steam.Abstractions;

namespace SProject.Steam.Tests;

[TestFixture]
public class SteamClientFinderTest
{
    private static IOptions<SteamOptions> CreateSteamOptions(AbsenceSteamClientBehavior behavior)
    {
        var options = new SteamOptions
        {
            AbsenceSteamClientBehavior = behavior,
            SteamPathNodes = new List<SteamPathNode>
            {
                CreateSteamPathNode()
            }
        };
        return new OptionsWrapper<SteamOptions>(options);
    }

    private static SteamPathNode CreateSteamPathNode()
    {
        return new SteamPathNode
        {
            Name = "SteamPath",
            Path = @"Software\Valve\Steam",
            PathHive = RegistryHive.CurrentUser
        };
    }

    [Test]
    public void FindSteamClient_WhenCorrectRegistryPath_ReturnsValidDirectory()
    {
        // Arrange
        var existsDirectory = TestHelper.FileSystemInfoCreator.CreateDirectory();
        var options = CreateSteamOptions(AbsenceSteamClientBehavior.Ignore);
        var mockResolver = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mockResolver.Setup(x => x.GetInstallPath(options.Value.SteamPathNodes[0])).Returns(existsDirectory.FullName);
        var steamClientFinder = new SteamClientFinder(options, mockResolver.Object);

        // Act
        var directory = steamClientFinder.FindSteamClient();

        // Assert
        Assert.That(directory, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(directory.WorkingDirectory, Is.Not.Null);
            Assert.That(directory.WorkingDirectory.FullName, Is.EqualTo(existsDirectory.FullName));
            Assert.That(directory.IsRootDirectory, Is.True);
        });

        // Cleanup
        existsDirectory.Delete(true);
    }

    [Test]
    public void FindSteamClient_WhenDirectoryNotFound_ReturnsNull()
    {
        // Arrange
        var options = CreateSteamOptions(AbsenceSteamClientBehavior.Ignore);
        var mockResolver = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mockResolver.Setup(x => x.GetInstallPath(options.Value.SteamPathNodes[0])).Returns(default(string));
        var steamClientFinder = new SteamClientFinder(options, mockResolver.Object);

        // Act
        var steamClient = steamClientFinder.FindSteamClient();

        // Assert
        Assert.That(steamClient, Is.Null);
    }

    [TestCase(AbsenceSteamClientBehavior.Throw, typeof(SteamClientNotFoundException))]
    [TestCase((AbsenceSteamClientBehavior) 2023, typeof(InvalidEnumArgumentException))]
    public void FindSteamClient_WithInvalidBehavior_ThrowsException(AbsenceSteamClientBehavior behavior, Type exceptionType)
    {
        // Arrange
        var options = CreateSteamOptions(behavior);

        var mockResolver = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mockResolver.Setup(x => x.GetInstallPath(options.Value.SteamPathNodes[0])).Returns(default(string));

        var steamClientFinder = new SteamClientFinder(options, mockResolver.Object);

        // Act & Assert
        Assert.Throws(exceptionType, () => steamClientFinder.FindSteamClient());
    }
}