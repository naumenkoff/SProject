using Microsoft.Extensions.DependencyInjection;

namespace SProject.Steam.Tests;

[TestFixture]
[TestOf(typeof(SteamClientServiceCollectionExtensions))]
public class SteamClientServiceCollectionExtensionsTest
{
    [Test]
    public void AddSteamClient_ReturnsServiceCollection_WithAddedDependencies()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddSteamClient();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(x => x.ServiceType == typeof(ISteamClientFinder)));
            Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(x => x.ServiceType == typeof(ISteamInstallPathResolver<SteamPathNode>)));
        });
    }
}