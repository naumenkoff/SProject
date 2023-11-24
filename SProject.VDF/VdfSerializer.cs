namespace SProject.VDF;

public class VdfSerializer
{
    /*T ParseVdf<T>(StreamReader stream)
    {
        T instance = Activator.CreateInstance<T>();
        if (instance is null) throw new NotImplementedException();

        var parsingResult = Parse(stream, string.Empty);
        var fields = instance.GetType().GetProperties().ToDictionary(x => x.Name.ToLower(), y => y);

        SetValues<T>(parsingResult, fields, instance);

        return instance;
    }*/

    /*void SetValues<T>(VdfObjectRoot vdfObjectRoot, Dictionary<string, PropertyInfo> dictionary, T instance)
    {
        foreach (var vdfObject in vdfObjectRoot.VObjects)
        {
            if (vdfObject is VdfObject valueObject)
            {
                if (dictionary.TryGetValue(valueObject.Key, out var property))
                {
                    if (property.PropertyType == typeof(bool)) { property.SetValue(instance, valueObject.Value is "1", null); }
                    else
                        property.SetValue(instance, Convert.ChangeType(valueObject.Value, property.PropertyType), null);
                }
            }
            else if (vdfObject is VdfObjectRoot rootObject)
            {
                if (string.IsNullOrEmpty(rootObject.Key))
                    SetValues(rootObject, dictionary, instance);
                else
                {
                    if (dictionary.TryGetValue(rootObject.Key, out var property))
                    {
                        if (pro)
                    }
                }
            }
        }
    }*/

    public static RootObject Parse(StreamReader streamReader)
    {
        return Parse(streamReader, string.Empty);
    }

    private static RootObject Parse(StreamReader stream, string? key)
    {
        var root = new RootObject
        {
            Key = key?.ToLower()
        };
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;

            if (IsHeader(line))
            {
                stream.ReadLine();

                var list = Parse(stream, ExtractKeyValue(line).key);
                root.ValueObjects.Add(list.Key!, new RootObject
                {
                    Key = list.Key,
                    ValueObjects = list.ValueObjects
                });

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