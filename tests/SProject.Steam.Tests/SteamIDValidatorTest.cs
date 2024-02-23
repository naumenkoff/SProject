namespace SProject.Steam.Tests;

[TestFixture]
[TestOf(typeof(SteamIDValidator))]
public class SteamIDValidatorTest
{
    [Test]
    public void IsSteamID64_FromCorrectString_ReturnsTrue()
    {
        // Arrange
        const string id64 = "76561198073887158";

        // Act
        var result = SteamIDValidator.IsSteamID64(id64);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSteamID64_FromIncorrectString_ReturnsFalse()
    {
        // Arrange
        const string id64 = "Hello, World!";

        // Act
        var result = SteamIDValidator.IsSteamID64(id64);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsSteamID64_FromCorrectLong_ReturnsTrue()
    {
        // Arrange
        const long id64 = 76561198073887158;

        // Act
        var result = SteamIDValidator.IsSteamID64(id64);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSteamID64_FromIncorrectLong_ReturnsTrue()
    {
        // Arrange
        const long id64 = 113621430;

        // Act
        var result = SteamIDValidator.IsSteamID64(id64);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsSteamID32_FromCorrectString_ReturnsTrue()
    {
        // Arrange
        const string id32 = "113621430";

        // Act
        var result = SteamIDValidator.IsSteamID32(id32);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSteamID32_FromIncorrectString_ReturnsFalse()
    {
        // Arrange
        const string id32 = "Hello, World!";

        // Act
        var result = SteamIDValidator.IsSteamID32(id32);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsSteamID32_FromUInt_ReturnsTrue()
    {
        // Arrange
        const uint id32 = 113621430;

        // Act
        var result = SteamIDValidator.IsSteamID32(id32);

        // Assert
        Assert.That(result, Is.True);

        // idk about id32 limits so
    }
}