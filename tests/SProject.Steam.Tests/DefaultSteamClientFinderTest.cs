using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Moq;
using SProject.TestHelper;

namespace SProject.Steam.Tests;

[TestFixture]
public class DefaultSteamClientFinderTest
{
    private static IOptions<SteamOptions> CreateSteamOptions(bool throwOnAbsence)
    {
        var options = new SteamOptions
        {
            ThrowOnAbsence = throwOnAbsence,
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
        var existsDirectory = FileSystemInfoCreator.CreateDirectory();
        var options = CreateSteamOptions(false);
        var mockResolver = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mockResolver.Setup(x => x.GetInstallPath(options.Value.SteamPathNodes[0])).Returns(existsDirectory.FullName);
        var steamClientFinder = new DefaultSteamClientFinder(options, mockResolver.Object);

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
        var options = CreateSteamOptions(false);
        var mockResolver = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mockResolver.Setup(x => x.GetInstallPath(options.Value.SteamPathNodes[0])).Returns(default(string));
        var steamClientFinder = new DefaultSteamClientFinder(options, mockResolver.Object);

        // Act
        var steamClient = steamClientFinder.FindSteamClient();

        // Assert
        Assert.That(steamClient, Is.Null);
    }

    [TestCase(true, typeof(DirectoryNotFoundException))]
    public void FindSteamClient_WithInvalidBehavior_ThrowsException(bool throwOnAbsence, Type exceptionType)
    {
        // Arrange
        var options = CreateSteamOptions(throwOnAbsence);

        var mockResolver = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mockResolver.Setup(x => x.GetInstallPath(options.Value.SteamPathNodes[0])).Returns(default(string));

        var steamClientFinder = new DefaultSteamClientFinder(options, mockResolver.Object);

        // Act & Assert
        Assert.Throws(exceptionType, () => steamClientFinder.FindSteamClient());
    }
}