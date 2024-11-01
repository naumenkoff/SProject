using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SProject.Math;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class Vector2Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Truncate(this Vector2 vector2)
    {
        return new Vector2(float.Truncate(vector2.X), float.Truncate(vector2.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Floor(this Vector2 vector2)
    {
        return new Vector2(float.Floor(vector2.X), float.Floor(vector2.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(this Vector2 vector2)
    {
        return new Vector2(float.Round(vector2.X), float.Round(vector2.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Lerp(this Vector2 vector2, double value)
    {
        return vector2 * (float)double.Clamp(value, 0.0, 1.0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Lerp(this Vector2 vector2, float value)
    {
        return vector2 * float.Clamp(value, 0f, 1f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Scale(this Vector2 vector2, float xValue, float yValue)
    {
        return new Vector2(vector2.X * xValue, vector2.Y * yValue);
    }
}