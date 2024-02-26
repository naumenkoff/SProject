using System.Text;
using SProject.VDF.Collections;
using SProject.VDF.Extensions;

namespace SProject.VDF;

public static class ValveDataFileParser
{
    public static ValveDataDocument Parse(ReadOnlySpan<byte> buffer)
    {
        var properties = new ValveDataCollection<ValveDataProperty>();
        var sections = new ValveDataCollection<ValveDataSection>();
        return new ValveDataDocument
        {
            PrimarySection = Read(buffer, sections, properties),
            Sections = sections,
            Properties = properties
        };
    }

    public static ValveDataDocument Parse(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenRead();
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Parse(memoryStream.GetBuffer());
    }

    public static ValveDataDocument Parse(string path)
    {
        var fileInfo = new FileInfo(path);
        return Parse(fileInfo);
    }

    private static ValveDataSection? Read(ReadOnlySpan<byte> buffer,
        ValveDataCollection<ValveDataSection> sections,
        ValveDataCollection<ValveDataProperty> properties)
    {
        var primarySection = new ValveDataSection(null!);
        var sectionKey = default(string);

        var stack = new Stack<ValveDataSection>();
        stack.Push(primarySection);

        var index = 0;
        while (index < buffer.Length)
        {
            var value = buffer[index];
            var parentSection = stack.Peek();

            // If the current byte is '{'
            if (value.IsOpeningCurlyBrace())
            {
                // First, we need to determine the key of this container
                // Since we have a structure like "Key" { "Values" }
                // If we got here, and the key is null or empty,
                // it means that current document is incorrect
                ArgumentException.ThrowIfNullOrEmpty(sectionKey);

                var childSection = new ValveDataSection(sectionKey);
                stack.Push(childSection);
                parentSection.Add(childSection);

                index++;
                continue;
            }

            // If the current byte is '}'
            if (value.IsClosingCurlyBrace())
            {
                // if (stack.Pop() == parentSection) sections.Add(parentSection); 
                // else throw new InvalidOperationException();
                sections.Add(stack.Pop());

                // To prevent exiting immediately upon leaving from nesting, increment the index.
                // End nesting
                index++;
                continue;
            }

            var kv = DetermineKeyValue(buffer, ref index);
            if (kv.key is null) continue;
            if (kv.value is null)
            {
                sectionKey = kv.key;
            }
            else
            {
                var property = new ValveDataProperty(kv.key, kv.value);
                primarySection.Add(property);
                properties.Add(property);
            }
        }

        return primarySection.SingleOrDefault();
    }

    private static (string? key, string? value) DetermineKeyValue(ReadOnlySpan<byte> buffer, ref int index)
    {
        int keyStartIndex = 0, keyEndIndex = 0, valueStartIndex = 0;

        while (index < buffer.Length)
        {
            if (keyStartIndex == 0 &&
                (buffer[index].IsOpeningCurlyBrace() || buffer[index].IsClosingCurlyBrace())) break;
            if (!buffer[index++].IsDoubleQuote()) continue;

            if (keyStartIndex == 0 && (index == 1 || buffer[index - 2].IsTab()))
            {
                keyStartIndex = index;
            }
            else if (keyEndIndex == 0)
            {
                if (buffer[index].IsTab())
                {
                    keyEndIndex = index - 1;
                }
                else if (buffer[index].IsNewline())
                {
                    keyEndIndex = index - 1;
                    return (Encoding.ASCII.GetString(buffer.Slice(keyStartIndex, keyEndIndex - keyStartIndex)), null);
                }
            }
            else if (valueStartIndex == 0 && buffer[index - 2].IsTab())
            {
                valueStartIndex = index;
            }
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