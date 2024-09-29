namespace SProject.Steam.Tests;

[TestFixture]
[TestOf(typeof(SteamIDValidator))]
public class SteamIDValidatorTest
{
    [Test]
    [TestCase("76561198073887158")]
    public void IsSteamID64_FromCorrectString_ReturnsTrue(string value)
    {
        var result = SteamIDValidator.IsSteamID64(value);
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase("Hello, World!")]
    public void IsSteamID64_FromIncorrectString_ReturnsFalse(string value)
    {
        var result = SteamIDValidator.IsSteamID64(value);
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(76561198073887158)]
    public void IsSteamID64_FromCorrectLong_ReturnsTrue(long value)
    {
        var result = SteamIDValidator.IsSteamID64(value);
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase(113621430)]
    public void IsSteamID64_FromIncorrectLong_ReturnsTrue(long value)
    {
        var result = SteamIDValidator.IsSteamID64(value);
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("113621430")]
    public void IsSteamID32_FromCorrectString_ReturnsTrue(string value)
    {
        var result = SteamIDValidator.IsSteamID32(value);
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase("Hello, World!")]
    public void IsSteamID32_FromIncorrectString_ReturnsFalse(string value)
    {
        var result = SteamIDValidator.IsSteamID32(value);
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(113621430u)]
    public void IsSteamID32_FromUInt_ReturnsTrue(uint value)
    {
        var result = SteamIDValidator.IsSteamID32(value);
        Assert.That(result, Is.True);
    }
}