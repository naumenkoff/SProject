using System.Text;

namespace SProject.VDF;

public static class ByteVdfParser
{
    public static VdfNode Parse(ReadOnlySpan<byte> buffer)
    {
        var valueCollection = new VdfCollection<VdfValue>();
        var containerCollection = new VdfCollection<VdfContainer>();
        return new VdfNode
        {
            Root = Read(buffer, containerCollection, valueCollection),
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

    private static VdfContainer? Read(ReadOnlySpan<byte> buffer, VdfCollection<VdfContainer> containerCollection,
        VdfCollection<VdfValue> valueCollection)
    {
        var rootContainer = new VdfContainer(null!);
        string nestedContainerKey = null!;

        var stack = new Stack<VdfContainer>();
        stack.Push(rootContainer);

        var index = 0;
        while (index < buffer.Length)
        {
            var value = buffer[index];
            var container = stack.Peek();

            // If the current byte is '{'
            if (value.IsOpeningCurlyBrace())
            {
                // First, we need to determine the key of this container
                // Since we have a structure like "Key" { "Values" }
                // If we got here, and the key is null or empty,
                // it means that current document is incorrect
                ArgumentException.ThrowIfNullOrEmpty(nestedContainerKey);

                var newContainer = new VdfContainer(nestedContainerKey);
                stack.Push(newContainer);
                container.Containers.Add(newContainer);

                index++;
                continue;
            }

            // If the current byte is '}'
            if (value.IsClosingCurlyBrace())
            {
                var filledContainer = stack.Pop();
                containerCollection.Add(filledContainer);

                // To prevent exiting immediately upon leaving from nesting, increment the index.
                // End nesting
                index++;
                continue;
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

        return rootContainer.SingleOrDefault();
    }

    private static (string? key, string? value) DetermineKeyValue(ReadOnlySpan<byte> buffer, ref int index)
    {
        int keyStartIndex = 0, keyEndIndex = 0, valueStartIndex = 0;

        while (index < buffer.Length)
        {
            if (keyStartIndex == 0 && (buffer[index].IsOpeningCurlyBrace() || buffer[index].IsClosingCurlyBrace())) break;
            if (!buffer[index++].IsDoubleQuote()) continue;

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