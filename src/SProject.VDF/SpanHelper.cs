namespace SProject.VDF;

internal static class SpanHelper
{
    internal static int IndexOf(ReadOnlySpan<char> span, char to, int startPosition = 0)
    {
        for (var i = startPosition; i < span.Length; i++)
            if (span[i] == to)
                return i;

        return -1;
    }

    internal static int LastIndexOf(ReadOnlySpan<char> span, char to)
    {
        for (var i = span.Length - 1; i > 0; i--)
            if (span[i] == to)
                return i;

        return -1;
    }
}