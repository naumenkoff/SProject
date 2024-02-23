namespace SProject.Steam.Tests;

[TestFixture]
[TestOf(typeof(SteamConverter))]
public class SteamConverterTest
{
    [Test]
    public void ToSteamID64_FromSteamID32_ReturnsSteamID64()
    {
        // Arrange
        const uint id32 = 113621430;

        // Act
        var id64 = SteamConverter.ToSteamID64(id32);

        // Assert
        Assert.That(id64, Is.EqualTo(76561198073887158));
    }

    [Test]
    public void ToSteamID32_FromSteamID64_ReturnsSteamID32()
    {
        // Arrange
        const long id64 = 76561198073887158;

        // Act
        var id32 = SteamConverter.ToSteamID32(id64);

        // Assert
        Assert.That(id32, Is.EqualTo(113621430));
    }

    [Test]
    public void ToSteamPermanentUrl_FromSteamID64_ReturnsSteamPermanentUrl()
    {
        // Arrange
        const long id64 = 76561198073887158;

        // Act
        var url = SteamConverter.ToSteamPermanentUrl(id64);

        // Assert
        Assert.That(url, Is.EqualTo("https://steamcommunity.com/profiles/76561198073887158"));
    }

    [Test]
    public void ToSteamID3_FromSteamID32_ReturnsSteamID3()
    {
        // Arrange
        const uint id32 = 113621430;

        // Act
        var id3 = SteamConverter.ToSteamID3(id32);

        // Assert
        Assert.That(id3, Is.EqualTo("[U:1:113621430]"));
    }

    [Test]
    public void ToSteamID_FromSteamID64_ReturnsSteamID()
    {
        // Arrange
        const long id64 = 76561198073887158;

        // Act
        var id = SteamConverter.ToSteamID(id64, out var instance, out var type);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(id, Is.EqualTo("STEAM_1:0:56810715"));
            Assert.That(instance, Is.EqualTo(1));
            Assert.That(type, Is.EqualTo(1));
        });
    }

    [Test]
    public void ToSteamID64_FromSteamID_ReturnsSteamID64()
    {
        // Arrange
        const byte y = 0;
        const int z = 56810715;

        // Act
        var id64 = SteamConverter.ToSteamID64(y, z);

        // Assert
        Assert.That(id64, Is.EqualTo(76561198073887158));
    }
}