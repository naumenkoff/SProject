using System.Text;
using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class ByteVdfParser
{
    public static VdfNode Parse(ReadOnlySpan<byte> buffer)
    {
        var valueCollection = new VdfCollection<VdfValue>();
        var containerCollection = new VdfCollection<VdfContainer>();
        Read(buffer, null!, containerCollection, valueCollection, out var rootContainer);
        return new VdfNode
        {
            Root = rootContainer.SingleOrDefault(),
            AllContainers = containerCollection,
            AllObjects = valueCollection
        };
    }

    public static VdfNode Parse(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenRead();
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Parse(memoryStream.GetBuffer());
    }

    public static VdfNode Parse(string path)
    {
        var fileInfo = new FileInfo(path);
        return Parse(fileInfo);
    }

    private static int Read(ReadOnlySpan<byte> buffer, string key, VdfCollection<VdfContainer> containerCollection,
        VdfCollection<VdfValue> valueCollection, out VdfContainer rootContainer)
    {
        rootContainer = new VdfContainer(key);
        string nestedContainerKey = null!;

        var index = 0;
        while (index < buffer.Length)
        {
            var value = buffer[index];

            // If the current byte is '{'
            if (value.IsOpeningCurlyBrace())
            {
                // First, we need to determine the key of this container
                // Since we have a structure like "Key" { "Values" }
                // If we got here, and the key is null or empty,
                // it means that current document is incorrect
                ArgumentException.ThrowIfNullOrEmpty(nestedContainerKey);

                // We are currently at '{', we need to move forward to avoid getting stuck in a loop
                index++;

                // Reading the nested node
                index += Read(buffer[index..], nestedContainerKey, containerCollection, valueCollection, out var vdfContainer);

                // Adding a node to the tree of the current container and to the list of all containers
                rootContainer.Containers.Add(vdfContainer);
                containerCollection.Add(vdfContainer);
            }

            // If the current byte is '}'
            if (value.IsClosingCurlyBrace())
            {
                // To prevent exiting immediately upon leaving from nesting, increment the index.
                index++;

                // End nesting
                break;
            }

            var kv = DetermineKeyValue(buffer, ref index);
            if (kv.key is null) continue;
            if (kv.value is null)
                nestedContainerKey = kv.key;
            else
            {
                var vdfValue = new VdfValue(kv.key, kv.value);
                rootContainer.Objects.Add(vdfValue);
                valueCollection.Add(vdfValue);
            }
        }

        return index;
    }

    private static (string? key, string? value) DetermineKeyValue(ReadOnlySpan<byte> buffer, ref int index)
    {
        int keyStartIndex = 0, keyEndIndex = 0, valueStartIndex = 0;

        while (index < buffer.Length)
        {
            var value = buffer[index];

            if (keyStartIndex == 0 && (value.IsOpeningCurlyBrace() || value.IsClosingCurlyBrace())) break;

            index++;

            if (!value.IsDoubleQuote()) continue;

            if (keyStartIndex == 0 && (index == 1 || buffer[index - 2].IsTab()))
                keyStartIndex = index;
            else if (keyEndIndex == 0)
            {
                if (buffer[index].IsTab())
                    keyEndIndex = index - 1;
                else if (buffer[index].IsNewline())
                {
                    keyEndIndex = index - 1;
                    return (Encoding.ASCII.GetString(buffer.Slice(keyStartIndex, keyEndIndex - keyStartIndex)), null);
                }
            }
            else if (valueStartIndex == 0 && buffer[index - 2].IsTab())
                valueStartIndex = index;
            else if (buffer[index].IsNewline())
            {
                var valueEndIndex = index - 1;
                return (Encoding.ASCII.GetString(buffer.Slice(keyStartIndex, keyEndIndex - keyStartIndex)),
                    Encoding.ASCII.GetString(buffer.Slice(valueStartIndex, valueEndIndex - valueStartIndex)));
            }
        }

        return (null, null);
    }
}