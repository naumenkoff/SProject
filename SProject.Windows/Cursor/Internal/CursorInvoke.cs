using System.Runtime.InteropServices;

namespace SProject.Windows.Cursor.Internal;

internal static partial class CursorInvoke
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetCursorInfo(ref CursorInfo cursorInfo);
}