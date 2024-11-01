using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Moq;
using SProject.Steam.Abstractions;

namespace SProject.Steam.Tests;

[TestFixture]
[Platform("Win")]
[TestOf(typeof(DefaultSteamClientFinder))]
public sealed class DefaultSteamClientFinderTest
{
    private static readonly SteamPathNode SomeNode = new()
    {
        Name = "Name",
        Path = "Path",
        PathHive = RegistryHive.ClassesRoot
    };

    [Test]
    public void FindSteamClients_ReturnEmptyEnumerable_WhenEmptyOptions()
    {
        // Arrange
        var steamOptions = new SteamOptions();
        var steamOptionsWrapper = new OptionsWrapper<SteamOptions>(steamOptions);
        var mock = new Mock<ISteamInstallPathResolver<SteamPathNode>>(MockBehavior.Strict);
        var steamClientFinder = new DefaultSteamClientFinder(steamOptionsWrapper, mock.Object, NullLogger<DefaultSteamClientFinder>.Instance);

        // Act
        var steamClients = steamClientFinder.FindSteamClients().ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mock.Invocations, Is.Empty);
            Assert.That(steamClients, Is.Empty);
        });
    }

    [Test]
    public void FindSteamClients_ReturnsEmpty_WhenPathResolverReturnsNull()
    {
        // Arrange
        var steamOptions = new SteamOptions { SteamPathNodes = [SomeNode] };
        var steamOptionsWrapper = new OptionsWrapper<SteamOptions>(steamOptions);
        var mock = new Mock<ISteamInstallPathResolver<SteamPathNode>>(MockBehavior.Loose);
        var steamClientFinder = new DefaultSteamClientFinder(steamOptionsWrapper, mock.Object, NullLogger<DefaultSteamClientFinder>.Instance);

        // Act
        var steamClients = steamClientFinder.FindSteamClients().ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mock.Invocations, Has.One.Items);
            Assert.That(steamClients, Is.Empty);
        });
    }

    [Test]
    public void FindSteamClients_ReturnsEmpty_WhenDirectoryNotExist()
    {
        // Arrange
        var steamOptions = new SteamOptions { SteamPathNodes = [SomeNode] };
        var steamOptionsWrapper = new OptionsWrapper<SteamOptions>(steamOptions);
        var mock = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mock.Setup(x => x.Resolve(It.IsAny<SteamPathNode>())).Returns(Path.GetRandomFileName);
        var steamClientFinder = new DefaultSteamClientFinder(steamOptionsWrapper, mock.Object, NullLogger<DefaultSteamClientFinder>.Instance);

        // Act
        var steamClients = steamClientFinder.FindSteamClients().ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mock.Invocations, Has.One.Items);
            Assert.That(steamClients, Is.Empty);
        });
    }

    [Test]
    public void FindSteamClients_ReturnsSteamClientModel_WhenDirectoryExist()
    {
        // Arrange
        var steamOptions = new SteamOptions { SteamPathNodes = [SomeNode] };
        var steamOptionsWrapper = new OptionsWrapper<SteamOptions>(steamOptions);
        var mock = new Mock<ISteamInstallPathResolver<SteamPathNode>>();
        mock.Setup(x => x.Resolve(It.IsAny<SteamPathNode>())).Returns(Path.GetTempPath);
        var steamClientFinder = new DefaultSteamClientFinder(steamOptionsWrapper, mock.Object, NullLogger<DefaultSteamClientFinder>.Instance);

        // Act
        var steamClients = steamClientFinder.FindSteamClients().ToArray();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(steamClients, Has.One.Items);
            Assert.That(steamClients, Has.All.Matches<SteamClientModel>(client => client.WorkingDirectory.FullName == Path.GetTempPath()));
        });
    }
}