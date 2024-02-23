using System.Numerics;

namespace SProject.Math.Tests;

[TestFixture]
[TestOf(typeof(Vector2Extensions))]
public class Vector2ExtensionsTests
{
    [Test]
    public void Floor_ReturnsFlooredVector()
    {
        // Arrange
        var vector2 = new Vector2((float) System.Math.PI, (float) System.Math.E);

        // Act
        var flooredVector2 = vector2.Floor();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(flooredVector2.X, Is.EqualTo(3));
            Assert.That(flooredVector2.Y, Is.EqualTo(2));
        });
    }

    [Test]
    public void Round_ReturnsRoundedVector()
    {
        // Arrange
        var vector2 = new Vector2((float) System.Math.PI, (float) System.Math.E);

        // Act
        var flooredVector2 = vector2.Round();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(flooredVector2.X, Is.EqualTo(3));
            Assert.That(flooredVector2.Y, Is.EqualTo(3));
        });
    }

    [Test]
    public void LerpDouble_ReturnsLerpedVector()
    {
        // Arrange
        var vector2 = new Vector2(-1, 1);

        // Act
        var lerpedVector = vector2.Lerp(0.5);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(lerpedVector.X, Is.EqualTo(-0.5f));
            Assert.That(lerpedVector.Y, Is.EqualTo(0.5f));
        });
    }

    [Test]
    public void LerpFloat_ReturnsLerpedVector()
    {
        // Arrange
        var vector2 = new Vector2(-1, 1);

        // Act
        var lerpedVector = vector2.Lerp(0.5f);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(lerpedVector.X, Is.EqualTo(-0.5f));
            Assert.That(lerpedVector.Y, Is.EqualTo(0.5f));
        });
    }

    [Test]
    public void ScaleX_ReturnsScaledVector()
    {
        // Arrange
        var vector2 = new Vector2(10, 10);

        // Act
        var scaledVector2 = vector2.ScaleX(10);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(scaledVector2.X, Is.EqualTo(100));
            Assert.That(scaledVector2.Y, Is.EqualTo(10));
        });
    }

    [Test]
    public void ScaleY_ReturnsScaledVector()
    {
        // Arrange
        var vector2 = new Vector2(10, 10);

        // Act
        var scaledVector2 = vector2.ScaleY(10);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(scaledVector2.X, Is.EqualTo(10));
            Assert.That(scaledVector2.Y, Is.EqualTo(100));
        });
    }

    [Test]
    public void Scale_ReturnsScaledVector()
    {
        // Arrange
        var vector2 = new Vector2(10, 10);

        // Act
        var scaledVector2 = vector2.Scale(10, 10);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(scaledVector2.X, Is.EqualTo(100));
            Assert.That(scaledVector2.Y, Is.EqualTo(100));
        });
    }
}