namespace SProject.VDF;

internal static class ByteExtensions
{
    public static bool IsTab(this byte value)
    {
        return value == 0x9;
    }

    public static bool IsNewline(this byte value)
    {
        return value == 10;
    }

    public static bool IsDoubleQuote(this byte value)
    {
        return value == 0x22;
    }

    public static bool IsOpeningCurlyBrace(this byte value)
    {
        return value == 0x7B;
    }

    public static bool IsClosingCurlyBrace(this byte value)
    {
        return value == 0x7D;
    }
}