using Microsoft.Win32;

namespace SProject.Steam.Tests;

[TestFixture]
[TestOf(typeof(DefaultSteamInstallPathResolver))]
public class DefaultSteamInstallPathResolverTest
{
    [Test]
    public void GetInstallPath_WithAnyExistingRegistryKey_ReturnsInstallPath()
    {
        // Arrange
        var pathResolver = new DefaultSteamInstallPathResolver();

        // Act
        var path = pathResolver.GetInstallPath(new SteamPathNode
        {
            Name = "Background",
            Path = "Control Panel\\Colors",
            PathHive = RegistryHive.CurrentUser
        });

        // Assert
        Assert.That(path, Is.Not.Null);
        Assert.That(path, Is.EqualTo("0 0 0"));
    }
}