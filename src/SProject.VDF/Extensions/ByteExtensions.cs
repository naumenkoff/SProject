using System.Runtime.CompilerServices;

namespace SProject.VDF.Extensions;

internal static class ByteExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDoubleQuote(this byte value)
    {
        return value == 0x22;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOpeningCurlyBrace(this byte value)
    {
        return value == 0x7B;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsClosingCurlyBrace(this byte value)
    {
        return value == 0x7D;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBackslash(this byte value)
    {
        return value == 0x5C;
    }
}