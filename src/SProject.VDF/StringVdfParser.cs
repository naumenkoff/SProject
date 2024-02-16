using System.Text;

namespace SProject.VDF;

public static class StringVdfParser
{
    public static VdfNode Parse(StreamReader streamReader)
    {
        var valueCollection = new VdfCollection<VdfValue>();
        var containerCollection = new VdfCollection<VdfContainer>();
        var rootContainer = Parse(streamReader, string.Empty, containerCollection, valueCollection);
        return new VdfNode
        {
            Root = rootContainer.SingleOrDefault(),
            AllContainers = containerCollection,
            AllObjects = valueCollection
        };
    }

    public static VdfNode Parse(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenText();
        return Parse(stream);
    }

    public static VdfNode Parse(string path, Encoding? encoding = null)
    {
        using var stream = new StreamReader(path, encoding ?? Encoding.UTF8);
        return Parse(stream);
    }

    private static VdfContainer Parse(StreamReader stream, string key, VdfCollection<VdfContainer> containerCollection,
        VdfCollection<VdfValue> valueCollection)
    {
        var root = new VdfContainer(key);

        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;

            if (IsHeader(line))
            {
                var keyValue = KeyValueExtractor.ExtractKeyValue(line);
                if (string.IsNullOrEmpty(keyValue.key)) continue;

                // "SomeKey" <-- We Are Here
                // { <-- Skip
                // ... <-- Parse
                // } <-- returns vdfContainer
                stream.ReadLine();

                // Adding VdfContainer to current container and shared container
                var vdfContainer = Parse(stream, keyValue.key, containerCollection, valueCollection);
                root.Containers.Add(vdfContainer);
                containerCollection.Add(vdfContainer);

                continue;
            }

            if (line[^1] == '}') return root;

            var (valueKey, value) = KeyValueExtractor.ExtractKeyValue(line);
            if (valueKey is null || value is null) continue;

            // Adding VdfValue to current container and shared container
            var vdfValue = new VdfValue(valueKey, value);
            root.Objects.Add(vdfValue);
            valueCollection.Add(vdfValue);
        }

        return root;
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