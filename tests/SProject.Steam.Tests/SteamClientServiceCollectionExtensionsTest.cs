using Microsoft.Extensions.DependencyInjection;
using SProject.Steam.Abstractions;

namespace SProject.Steam.Tests;

[TestFixture]
[Platform("Win")]
[TestOf(typeof(SteamClientServiceCollectionExtensions))]
public sealed class SteamClientServiceCollectionExtensionsTest
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
            Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(x => x.ServiceType == typeof(ISteamClientFinder) &&
                                                                                   x.ImplementationType == typeof(DefaultSteamClientFinder)));
            Assert.That(serviceCollection,
                        Has.One.Matches<ServiceDescriptor>(x => x.ServiceType == typeof(ISteamInstallPathResolver<SteamPathNode>) &&
                                                                x.ImplementationType == typeof(DefaultSteamInstallPathResolver)));
        });
    }
}