namespace SProject.VDF;

internal static class KeyValueExtractor
{
    internal static (string? key, string? value) ExtractKeyValue(ReadOnlySpan<char> line)
    {
        //      |
        //		"ClientID"		"6877367328171335534"
        var keyOpenBracketIndex = SpanHelper.IndexOf(line, '"');
        if (keyOpenBracketIndex == -1) return (null, null);

        //       |
        //		"ClientID"		"6877367328171335534"
        var keyContentStart = keyOpenBracketIndex + 1;

        //               |
        //		"ClientID"		"6877367328171335534"
        var keyCloseBracketIndex = SpanHelper.IndexOf(line, '"', keyContentStart);
        if (keyCloseBracketIndex == -1) return (null, null);

        //                |
        //		"ClientID"		"6877367328171335534"
        var keyContentEnd = keyCloseBracketIndex + 1;

        //      |ClientID|
        //		"ClientID"		"6877367328171335534"
        var valueKey = line.Slice(keyContentStart, keyCloseBracketIndex - keyContentStart);

        //                      |
        //		"ClientID"		"6877367328171335534"
        var valueOpenBracketIndex = SpanHelper.IndexOf(line, '"', keyContentEnd);
        if (valueOpenBracketIndex == -1) return (valueKey.ToString(), null);

        //                       |
        //		"ClientID"		"6877367328171335534"
        var valueContentStart = valueOpenBracketIndex + 1;

        //                                          |
        //		"ClientID"		"6877367328171335534"
        var valueCloseBracketIndex = SpanHelper.LastIndexOf(line, '"');
        if (valueCloseBracketIndex == -1 || valueCloseBracketIndex == valueOpenBracketIndex) return (valueKey.ToString(), null);

        //                      |6877367328171335534|
        //		"ClientID"		"6877367328171335534"
        var value = line.Slice(valueContentStart, valueCloseBracketIndex - valueContentStart);

        return (valueKey.ToString(), value.ToString());
    }
}