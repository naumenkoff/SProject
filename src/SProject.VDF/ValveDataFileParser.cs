using System.Buffers;
using System.Runtime.CompilerServices;
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

    public static ValveDataDocument Parse(FileInfo fileInfo)
    {
        var size = fileInfo.Length >= 4096 ? 4096 : fileInfo.Length;
        using var stream = new FileStream(
            fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, (int)size, false);
        return Parse(stream);
    }

    public static ValveDataDocument Parse(string path)
    {
        var fileInfo = new FileInfo(path);
        return Parse(fileInfo);
    }

    private static ValveDataSection? ReadStream(Stream stream,
        ValveDataCollection<ValveDataSection> sections,
        ValveDataCollection<ValveDataProperty> properties)
    {
        var primarySection = new ValveDataSection(null!);
        var stack = new Stack<ValveDataSection>();
        stack.Push(primarySection);

        const int bufferSize = 2048;

        var isEscaped = false;
        var useLists = stream.Length >= bufferSize;
        var keyStart = -1;
        var keyEnd = -1;
        var valueStart = -1;
        var position = ArrayPool<byte>.Shared.Rent(useLists ? bufferSize : (int)stream.Length);
        if (useLists)
        {
            var i = 0;
            var offset = 0;
            while (stream.Position < stream.Length)
            {
                var bytesRead = offset + stream.Read(position, offset, position.Length - offset);

                var buffer = position.AsSpan();
                for (; i < bytesRead; i++)
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

                        var property = CreateProperty(buffer.Slice(keyStart, keyEnd - keyStart),
                            buffer.Slice(valueStart, i - valueStart));

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

                if (keyStart == -1)
                {
                    i = 0;
                    offset = 0;
                }
                else
                {
                    ArrayPool<byte>.Shared.Return(position);

                    // 512 - 256 = 256
                    offset = buffer.Length - keyStart;

                    // 256 + 512
                    position = ArrayPool<byte>.Shared.Rent(offset + bufferSize);

                    // 256..512
                    buffer.Slice(keyStart, offset).CopyTo(position);

                    // keyEnd = 260 - 256 = 4
                    if (keyEnd != -1) keyEnd -= keyStart;

                    // valueStart = 280 - 256 = 24
                    if (valueStart != -1) valueStart -= keyStart;

                    i = offset;
                    keyStart = 0;
                }
            }
        }
        else
        {
            // DRY Violation
            var bytesRead = stream.Read(position, 0, position.Length);
            var buffer = position.AsSpan();
            for (var i = 0; i < bytesRead; i++)
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

                    var property = CreateProperty(buffer.Slice(keyStart, keyEnd - keyStart),
                        buffer.Slice(valueStart, i - valueStart));

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
        }

        ArrayPool<byte>.Shared.Return(position);
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