namespace SProject.Steam.Tests;

[TestFixture]
[TestOf(typeof(SteamConverter))]
public class SteamConverterTest
{
    [Test]
    [TestCase(113621430u, 76561198073887158)]
    public void ToSteamID64_FromSteamID32_ReturnsSteamID64(uint value, long expected)
    {
        var result = SteamConverter.ToSteamID64(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(76561198073887158, 113621430u)]
    public void ToSteamID32_FromSteamID64_ReturnsSteamID32(long value, uint expected)
    {
        var result = SteamConverter.ToSteamID32(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(76561198073887158, "https://steamcommunity.com/profiles/76561198073887158")]
    public void ToSteamPermanentUrl_FromSteamID64_ReturnsSteamPermanentUrl(long value, string expected)
    {
        var result = SteamConverter.ToSteamPermanentUrl(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(113621430u, "[U:1:113621430]")]
    public void ToSteamID3_FromSteamID32_ReturnsSteamID3(uint value, string expected)
    {
        var result = SteamConverter.ToSteamID3(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(76561198073887158, "STEAM_1:0:56810715", 1, 1)]
    public void ToSteamID_FromSteamID64_ReturnsSteamID(long id64, string expectedId, int expectedInstance, int expectedT)
    {
        var id = SteamConverter.ToSteamID(id64, out var instance, out var type);
        Assert.Multiple(() =>
        {
            Assert.That(id, Is.EqualTo(expectedId));
            Assert.That(instance, Is.EqualTo(expectedInstance));
            Assert.That(type, Is.EqualTo(expectedT));
        });
    }

    [Test]
    [TestCase(0, 56810715, 76561198073887158)]
    public void ToSteamID64_FromSteamID_ReturnsSteamID64(byte y, int z, long expected)
    {
        var result = SteamConverter.ToSteamID64(y, z);
        Assert.That(result, Is.EqualTo(expected));
    }
}