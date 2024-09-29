using System.Runtime.CompilerServices;
using System.Text;
using SProject.VDF.Collections;
using SProject.VDF.Parsing;

namespace SProject.VDF;

public static class ValveDataFileParser
{
    #region Settings

    /// <summary>
    ///     | BufferSize | Mean     | Median   | Gen0    | Gen1   | Allocated |
    ///     |-----------:|---------:|---------:|--------:|-------:|----------:|
    ///     | 1024       | 75.23 us | 74.76 us | 13.0615 | 2.5635 |  80.63 KB |
    ///     | 2048       | 65.69 us | 65.70 us | 13.1836 | 3.1738 |  81.15 KB |
    ///     | 4096       | 60.29 us | 60.20 us | 13.2446 | 2.6245 |  81.23 KB |
    ///     | 8192       | 60.25 us | 59.91 us | 14.1602 | 2.8076 |  87.03 KB |
    ///     | 16384      | 57.33 us | 57.19 us | 14.1602 | 2.8076 |  87.07 KB |
    /// </summary>
    private const int BufferSize = 4 * 1024;

    private static readonly Encoding DefaultEncoding = Encoding.ASCII;

    #endregion

    #region Entry

    public static ValveDataDocument Parse(Span<byte> buffer)
    {
        return Read(buffer);
    }

    public static ValveDataDocument Parse(Stream stream)
    {
        return ReadStream(stream);
    }

    public static ValveDataDocument Parse(FileInfo fileInfo)
    {
        using var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 0,
                                          FileOptions.None);
        return Parse(stream);
    }

    public static ValveDataDocument Parse(string path)
    {
        var fileInfo = new FileInfo(path);
        return Parse(fileInfo);
    }

    #endregion

    #region Core

    private static ValveDataDocument ReadStream(Stream stream)
    {
        var properties = new ValveDataCollection<ValveDataProperty>();
        var sections = new ValveDataCollection<ValveDataSection>();
        var primarySection = new ValveDataSection(null!);
        var stack = new Stack<ValveDataSection>();
        stack.Push(primarySection);

        var isEscaped = false;
        var storage = new PooledArray<byte>(stream.Length >= BufferSize ? BufferSize : (int)stream.Length);
        var valueStart = -1;
        var keyStart = -1;
        var keyEnd = -1;

        var index = 0;
        var offset = 0;
        while (stream.Position < stream.Length)
        {
            for (var bytesRead = offset + stream.Read(storage.Array, offset, storage.Array.Length - offset);
                 index < bytesRead;
                 index++)
            {
                var value = storage[index];

                if (isEscaped)
                {
                    // Looking for non-escaped '{', '}', '"'.
                    isEscaped = value.IsBackslash();
                    continue;
                }

                if (value.IsDoubleQuote())
                {
                    // Key Start
                    if (keyStart == -1)
                    {
                        keyStart = index + 1;
                        continue;
                    }

                    // Key End
                    if (keyEnd == -1)
                    {
                        keyEnd = index;
                        continue;
                    }

                    // Value Start
                    if (valueStart == -1)
                    {
                        valueStart = index + 1;
                        continue;
                    }

                    // Value End
                    var property = CreateProperty(storage.Span.Slice(keyStart, keyEnd - keyStart),
                                                  storage.Span.Slice(valueStart, index - valueStart));

                    stack.Peek().Add(property);
                    properties.Add(property);

                    keyStart = -1;
                    keyEnd = -1;
                    valueStart = -1;
                    continue;
                }

                if (valueStart == -1)
                {
                    // Section Start
                    if (value.IsOpeningCurlyBrace())
                    {
                        var childSection = CreateSection(storage.Span.Slice(keyStart, keyEnd - keyStart));
                        stack.Peek().Add(childSection);
                        stack.Push(childSection);
                        sections.Add(childSection);
                        keyStart = -1;
                        keyEnd = -1;
                        continue;
                    }

                    // Section End
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
                index = 0;
                offset = 0;
            }
            else
            {
                storage.Resize(keyStart, offset);
                if (keyEnd != -1) keyEnd -= keyStart;
                if (valueStart != -1) valueStart -= keyStart;
                index = offset;
                keyStart = 0;
            }
        }

        storage.Close();
        return new ValveDataDocument
        {
            PrimarySection = primarySection.SingleOrDefault(),
            Sections = sections,
            Properties = properties
        };
    }

    private static ValveDataDocument Read(ReadOnlySpan<byte> span)
    {
        var properties = new ValveDataCollection<ValveDataProperty>();
        var sections = new ValveDataCollection<ValveDataSection>();
        var primarySection = new ValveDataSection(null!);
        var stack = new Stack<ValveDataSection>();
        stack.Push(primarySection);

        var isEscaped = false;
        var valueStart = -1;
        var keyStart = -1;
        var keyEnd = -1;

        for (var i = 0; i < span.Length; i++)
        {
            var value = span[i];

            if (isEscaped)
            {
                // Looking for non-escaped '{', '}', '"'.
                isEscaped = value.IsBackslash();
                continue;
            }

            if (value.IsDoubleQuote())
            {
                // Key Start
                if (keyStart == -1)
                {
                    keyStart = i + 1;
                    continue;
                }

                // Key End
                if (keyEnd == -1)
                {
                    keyEnd = i;
                    continue;
                }

                // Value Start
                if (valueStart == -1)
                {
                    valueStart = i + 1;
                    continue;
                }

                // Value End
                var property = CreateProperty(span.Slice(keyStart, keyEnd - keyStart),
                                              span.Slice(valueStart, i - valueStart));

                stack.Peek().Add(property);
                properties.Add(property);

                keyStart = -1;
                keyEnd = -1;
                valueStart = -1;
                continue;
            }

            if (valueStart == -1)
            {
                // Section Start
                if (value.IsOpeningCurlyBrace())
                {
                    var childSection = CreateSection(span.Slice(keyStart, keyEnd - keyStart));
                    stack.Peek().Add(childSection);
                    stack.Push(childSection);
                    sections.Add(childSection);
                    keyStart = -1;
                    keyEnd = -1;
                    continue;
                }

                // Section End
                if (value.IsClosingCurlyBrace())
                {
                    _ = stack.Pop();
                    continue;
                }
            }

            isEscaped = value.IsBackslash();
        }

        return new ValveDataDocument
        {
            PrimarySection = primarySection.SingleOrDefault(),
            Sections = sections,
            Properties = properties
        };
    }

    #endregion

    #region Utilities

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ValveDataSection CreateSection(ReadOnlySpan<byte> keyBytes)
    {
        var key = DefaultEncoding.GetString(keyBytes);
        return new ValveDataSection(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ValveDataProperty CreateProperty(ReadOnlySpan<byte> keyBytes, ReadOnlySpan<byte> valueBytes)
    {
        var key = DefaultEncoding.GetString(keyBytes);
        var value = valueBytes.Length > 0 ? DefaultEncoding.GetString(valueBytes) : string.Empty;
        return new ValveDataProperty(key, value);
    }

    #endregion
}