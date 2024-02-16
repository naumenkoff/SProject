using System.Runtime.InteropServices;

namespace SProject.Windows.Cursor.Internal;

public struct CursorInfo
{
    public static readonly CursorInfo Empty = new CursorInfo
    {
        cbSize = Marshal.SizeOf(typeof(CursorInfo))
    };

    /// <summary>
    ///     The size of the structure, in bytes. The caller must set this to sizeof(CURSORINFO).
    /// </summary>
    public int cbSize;

    /// <summary>
    ///     The cursor state.
    /// </summary>
    public int flags;

    /// <summary>
    ///     A handle to the cursor.
    /// </summary>
    public nint hCursor;

    /// <summary>
    ///     A structure that receives the screen coordinates of the cursor.
    /// </summary>
    public Point ptScreenPos;
}