using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using SProject.VDF.Collections;
using SProject.VDF.Extensions;

namespace SProject.VDF;

public static class ValveDataFileParser
{
    public static ValveDataDocument Parse(Span<byte> buffer)
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

    public static ValveDataDocument Parse(Stream stream)
    {
        var properties = new ValveDataCollection<ValveDataProperty>();
        var sections = new ValveDataCollection<ValveDataSection>();
        return new ValveDataDocument
        {
            PrimarySection = ReadStream(stream, sections, properties),
            Sections = sections,
            Properties = properties
        };
    }

    public static ValveDataDocument Parse(FileInfo fileInfo, bool streaming = true)
    {
        var size = fileInfo.Length >= 4096 ? 4096 : fileInfo.Length;
        using var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, (int)size,
            false);
        if (streaming) return Parse(stream);

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Parse(memoryStream.GetBuffer());
    }

    public static ValveDataDocument Parse(string path, bool streaming = true)
    {
        var fileInfo = new FileInfo(path);
        return Parse(fileInfo, streaming);
    }

    private static ValveDataSection? ReadStream(Stream stream,
        ValveDataCollection<ValveDataSection> sections,
        ValveDataCollection<ValveDataProperty> properties)
    {
        var keyBytes = new List<byte>(32);
        var collectKey = false;

        var valueBytes = new List<byte>(32);
        var collectValue = false;

        var primarySection = new ValveDataSection(null!);
        var stack = new Stack<ValveDataSection>();
        stack.Push(primarySection);

        var isEscaped = false;
        var position = new byte[stream.Length >= 256 ? 256 : stream.Length];
        while (stream.Position < stream.Length)
        {
            var bytesRead = stream.Read(position, 0, position.Length);
            for (var i = 0; i < bytesRead; i++)
            {
                var value = position[i];

                if (!isEscaped)
                {
                    if (value.IsDoubleQuote()) // "
                    {
                        if (collectKey)
                        {
                            collectKey = false;
                            continue;
                        }

                        if (keyBytes.Count == 0)
                        {
                            collectKey = true;
                            continue;
                        }

                        if (collectValue)
                        {
                            var property = CreateProperty(CollectionsMarshal.AsSpan(keyBytes),
                                CollectionsMarshal.AsSpan(valueBytes));
                            stack.Peek().Add(property);
                            properties.Add(property);

                            keyBytes.Clear();
                            if (valueBytes.Count != 0) valueBytes.Clear();
                            collectValue = false;
                            continue;
                        }

                        if (valueBytes.Count == 0)
                        {
                            collectValue = true;
                            continue;
                        }
                    }

                    if (!collectValue)
                    {
                        if (value.IsOpeningCurlyBrace())
                        {
                            var childSection = CreateSection(CollectionsMarshal.AsSpan(keyBytes));
                            stack.Peek().Add(childSection);
                            stack.Push(childSection);
                            sections.Add(childSection);
                            keyBytes.Clear();
                            continue;
                        }

                        if (value.IsClosingCurlyBrace())
                        {
                            _ = stack.Pop();
                            continue;
                        }
                    }
                }

                if (collectKey) keyBytes.Add(value);

                if (collectValue) valueBytes.Add(value);

                isEscaped = value.IsBackslash();
            }
        }

        return primarySection.SingleOrDefault();
    }

    private static ValveDataSection? Read(Span<byte> buffer,
        ValveDataCollection<ValveDataSection> sections,
        ValveDataCollection<ValveDataProperty> properties)
    {
        var keyStart = -1;
        var keyEnd = -1;

        var valueStart = -1;

        var primarySection = new ValveDataSection(null!);
        var stack = new Stack<ValveDataSection>();
        stack.Push(primarySection);

        var isEscaped = false;
        for (var i = 0; i < buffer.Length; i++)
        {
            var value = buffer[i];

            if (isEscaped)
            {
                isEscaped = value.IsBackslash();
                continue;
            }

            if (value.IsDoubleQuote())
            {
                if (keyStart == -1)
                {
                    keyStart = i + 1;
                    continue;
                }

                if (keyEnd == -1)
                {
                    keyEnd = i;
                    continue;
                }

                if (valueStart == -1)
                {
                    valueStart = i + 1;
                    continue;
                }

                var property = new ValveDataProperty(
                    Encoding.ASCII.GetString(buffer.Slice(keyStart, keyEnd - keyStart)),
                    Encoding.ASCII.GetString(buffer.Slice(valueStart, i - valueStart)));

                stack.Peek().Add(property);
                properties.Add(property);

                keyStart = -1;
                keyEnd = -1;
                valueStart = -1;
                continue;
            }

            if (valueStart == -1)
            {
                if (value.IsOpeningCurlyBrace())
                {
                    var childSection = CreateSection(buffer.Slice(keyStart, keyEnd - keyStart));
                    stack.Peek().Add(childSection);
                    stack.Push(childSection);
                    sections.Add(childSection);
                    keyStart = -1;
                    keyEnd = -1;
                    continue;
                }

                if (value.IsClosingCurlyBrace())
                {
                    _ = stack.Pop();
                    continue;
                }
            }

            isEscaped = value.IsBackslash();
        }

        return primarySection.SingleOrDefault();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ValveDataSection CreateSection(Span<byte> keyBytes)
    {
        var key = Encoding.ASCII.GetString(keyBytes);
        return new ValveDataSection(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ValveDataProperty CreateProperty(Span<byte> keyBytes, Span<byte> valueBytes)
    {
        var key = Encoding.ASCII.GetString(keyBytes);
        var value = valueBytes.Length == 0 ? string.Empty : Encoding.ASCII.GetString(valueBytes);
        return new ValveDataProperty(key, value);
    }
}