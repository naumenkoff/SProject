namespace SProject.Math.Tests;

[TestFixture]
[TestOf(typeof(ByteUnitConverter))]
public class ByteUnitConverterTest
{
    [Test]
    public void MegabytesToBytes_ReturnsBytes()
    {
        // Arrange & Act
        var bytes = ByteUnitConverter.MegabytesToBytes(4);

        // Assert
        Assert.That(bytes, Is.EqualTo(4_194_304));
    }
}