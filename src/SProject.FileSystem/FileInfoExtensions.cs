using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SProject.FileSystem;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class FileInfoExtensions
{
    private static readonly FileStreamOptions DefaultStreamOptions = new()
    {
        PreallocationSize = 0,
        Access = FileAccess.Read,
        Mode = FileMode.Open,
        Options = FileOptions.Asynchronous,
        BufferSize = 0,
        Share = FileShare.Read
    };

    private static string ReadSmallText(FileStream fileStream, Encoding encoding)
    {
        var buffer = ArrayPool<byte>.Shared.Rent((int)fileStream.Length);
        try
        {
            var bytesRead = fileStream.Read(buffer, 0, buffer.Length);
            return encoding.GetString(buffer, 0, bytesRead);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static string ReadLargeText(FileStream fileStream, Encoding encoding)
    {
        var stringBuilder = new StringBuilder((int)fileStream.Length);
        var buffer = ArrayPool<byte>.Shared.Rent(4096);
        try
        {
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var getString = encoding.GetString(buffer, 0, bytesRead);
                stringBuilder.Append(getString);
            }

            return stringBuilder.ToString();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static string ReadAllText(this FileInfo? fileInfo, Encoding? encoding = null)
    {
        if (!fileInfo.Exists()) throw new ArgumentException($"{nameof(fileInfo)} must not be null and must refer to an existing file.");

        encoding ??= Encoding.UTF8;
        using var fileStream = fileInfo.Open(DefaultStreamOptions);
        return fileStream.Length <= 4096 ? ReadSmallText(fileStream, encoding) : ReadLargeText(fileStream, encoding);
    }
}