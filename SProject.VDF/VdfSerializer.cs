using System.Text;
using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class VdfSerializer
{
    public static IRootObject Parse(StreamReader streamReader)
    {
        return Parse(streamReader, string.Empty);
    }

    public static IRootObject Parse(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenText();
        return Parse(stream);
    }

    public static IRootObject Parse(string path, Encoding? encoding = null)
    {
        using var stream = new StreamReader(path, encoding ?? Encoding.UTF8);
        return Parse(stream);
    }

    private static IRootObject Parse(StreamReader stream, string key)
    {
        var root = new RootObject(key);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;

            if (IsHeader(line))
            {
                stream.ReadLine();

                var list = Parse(stream, ExtractKeyValue(line).key);
                root.RootObjects.Add(list.Key!, list);

                continue;
            }

            if (line[^1] == '}') return root;

            var (valueKey, value) = ExtractKeyValue(line);
            if (valueKey is null || value is null) continue;

            root.ValueObjects.Add(valueKey, new ValueObject(valueKey, value));
        }

        return root;
    }

    private static (string? key, string? value) ExtractKeyValue(ReadOnlySpan<char> line)
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
        if (valueCloseBracketIndex == -1 || valueCloseBracketIndex == valueOpenBracketIndex) return (null, null);

        //                      |6877367328171335534|
        //		"ClientID"		"6877367328171335534"
        var value = line.Slice(valueContentStart, valueCloseBracketIndex - valueContentStart);

        return (valueKey.ToString(), value.ToString());
    }

    private static bool IsHeader(ReadOnlySpan<char> input)
    {
        var count = 0;

        foreach (var c in input)
        {
            if (c is '"') count++;
            if (count > 2) return false;
        }

        return count == 2;
    }
}