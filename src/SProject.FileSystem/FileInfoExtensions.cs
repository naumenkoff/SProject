namespace SProject.FileSystem;

/// <summary>
///     Provides extension methods for working with <see cref="System.IO.FileInfo" /> instances.
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    ///     Reads all the text from a file.
    /// </summary>
    /// <param name="fileInfo">The <see cref="System.IO.FileInfo" /> instance representing the file to read.</param>
    /// <param name="throwException">Specifies whether to throw an exception on error. If false, returns null on error.</param>
    /// <returns>The content of the file as a string, or null if not found or <paramref name="throwException" /> is false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fileInfo" /> is null or does not exist.</exception>
    public static string? ReadAllText(this FileInfo? fileInfo, bool throwException = false)
    {
        if (fileInfo is null || !fileInfo.Exists)
        {
            if (throwException) throw new ArgumentNullException(nameof(fileInfo), "The parameter was null or did not exist in the file system.");
            return null;
        }

        try
        {
            using var streamReader = fileInfo.OpenText();
            return streamReader.ReadToEnd();
        }
        catch (Exception)
        {
            if (throwException) throw;
            return null;
        }
    }
}