using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Win32;

namespace SProject.Steam.Tests;

[TestFixture]
[Platform("Win")]
[TestOf(typeof(DefaultSteamInstallPathResolver))]
public sealed class DefaultSteamInstallPathResolverTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        using var hive = RegistryKey.OpenBaseKey(_steamPathNode.PathHive, RegistryView.Registry64);
        using var path = hive.CreateSubKey(_steamPathNode.Path, true);
        path.SetValue(_steamPathNode.Name, PathNodeValue);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        using var hive = RegistryKey.OpenBaseKey(_steamPathNode.PathHive, RegistryView.Registry64);
        hive.DeleteSubKey(_steamPathNode.Path);
    }

    private const string PathNodeValue = @"C:\Program Files (x86)\Steam";

    private readonly SteamPathNode _steamPathNode = new()
    {
        Name = "Steam",
        Path = "SProject",
        PathHive = RegistryHive.CurrentUser
    };

    private readonly DefaultSteamInstallPathResolver _defaultSteamInstallPathResolver = new(NullLogger<DefaultSteamInstallPathResolver>.Instance);

    [Test]
    public void Resolve_ShouldReturnValue_WhenRegistryKeyExists()
    {
        // Arrange & Act
        var path = _defaultSteamInstallPathResolver.Resolve(_steamPathNode);

        // Assert
        StringAssert.AreEqualIgnoringCase(PathNodeValue, path);
    }

    [TestCaseSource(nameof(InvalidArgumentTestCases))]
    public void Resolve_ShouldReturnNull_WhenInvalidParams(SteamPathNode steamPathNode)
    {
        // Arrange & Act
        var path = _defaultSteamInstallPathResolver.Resolve(steamPathNode);

        // Assert
        Assert.That(path, Is.Null);
    }

    private static IEnumerable<TestCaseData> InvalidArgumentTestCases()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        yield return new TestCaseData(new SteamPathNode
        {
            Name = null,
            Path = "Path",
            PathHive = RegistryHive.CurrentUser
        }).SetName("Resolve_ShouldReturnNull_WhenInvalidName");

        yield return new TestCaseData(new SteamPathNode
        {
            Name = "Name",
            Path = null,
            PathHive = RegistryHive.CurrentUser
        }).SetName("Resolve_ShouldReturnNull_WhenInvalidPath");

        yield return new TestCaseData(new SteamPathNode
        {
            Name = "Name",
            Path = "Path",
            PathHive = (RegistryHive)1
        }).SetName("Resolve_ShouldReturnNull_WhenInvalidRegistryHive");

        yield return new TestCaseData(null).SetName("Resolve_ShouldReturnNull_WhenProvidedNull");
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}