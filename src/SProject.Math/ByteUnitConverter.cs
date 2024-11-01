using System.Diagnostics.CodeAnalysis;

namespace SProject.Math;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ByteUnitConverter
{
#region From Bytes

    public static long BytesToKibibytes(float value)
    {
        return (long)(value / 1024);
    }

    public static long BytesToMebibytes(float value)
    {
        return (long)(value / (1024 * 1024));
    }

    public static long BytesToGibibytes(float value)
    {
        return (long)(value / (1024 * 1024 * 1024));
    }

#endregion

#region From Kibibytes

    public static long KibibytesToBytes(float value)
    {
        return (long)(value * 1024);
    }

    public static long KibibytesToMebibytes(float value)
    {
        return (long)(value / 1024);
    }

    public static long KibibytesToGibibytes(float value)
    {
        return (long)(value / (1024 * 1024));
    }

#endregion

#region From Mebibytes

    public static long MebibytesToBytes(float value)
    {
        return (long)(value * 1024 * 1024);
    }

    public static long MebibytesToKibibytes(float value)
    {
        return (long)(value * 1024);
    }

    public static long MebibytesToGibibytes(float value)
    {
        return (long)(value / 1024);
    }

#endregion

#region From Gibibytes

    public static long GibibytesToBytes(float value)
    {
        return (long)(value * 1024 * 1024 * 1024);
    }

    public static long GibibytesToKibibytes(float value)
    {
        return (long)(value * 1024 * 1024);
    }

    public static float GibibytesToMebibytes(float value)
    {
        return value * 1024;
    }

#endregion
}