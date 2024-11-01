namespace SProject.Math.Tests;

[TestFixture]
[TestOf(typeof(ByteUnitConverter))]
public class ByteUnitConverterTest
{
    [Test]
    public void MebibytesToBytes_ShouldReturnBytes()
    {
        // Arrange & Act
        var bytes = ByteUnitConverter.MebibytesToBytes(4);

        // Assert
        Assert.That(bytes, Is.EqualTo(4_194_304));
    }
}