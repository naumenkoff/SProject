using System.Numerics;

namespace SProject.Math;

public static class Vector2Extensions
{
    public static Vector2 Floor(this Vector2 vector2)
    {
        return new Vector2(float.Truncate(vector2.X), float.Truncate(vector2.Y));
    }

    public static Vector2 Round(this Vector2 vector2)
    {
        return new Vector2(float.Round(vector2.X), float.Round(vector2.Y));
    }

    public static Vector2 Lerp(this Vector2 vector2, double value)
    {
        return vector2 * (float) double.Clamp(value, 0.0, 1.0);
    }

    public static Vector2 Lerp(this Vector2 vector2, float value)
    {
        return vector2 * float.Clamp(value, 0f, 1f);
    }

    public static Vector2 ScaleX(this Vector2 vector2, float xValue)
    {
        return vector2 with
        {
            X = vector2.X * xValue
        };
    }

    public static Vector2 ScaleY(this Vector2 vector2, float yValue)
    {
        return vector2 with
        {
            Y = vector2.Y * yValue
        };
    }

    public static Vector2 Scale(this Vector2 vector2, float xValue, float yValue)
    {
        return new Vector2(vector2.X * xValue, vector2.Y * yValue);
    }
}