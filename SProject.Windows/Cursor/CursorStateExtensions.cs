using SProject.Windows.Cursor.Internal;

namespace SProject.Windows.Cursor;

public static class CursorStateExtensions
{
    public static bool IsShowing(this CursorState cursorState)
    {
        return cursorState == CursorState.Showing;
    }

    public static bool IsHidden(this CursorState cursorState)
    {
        return cursorState == CursorState.Hidden;
    }

    public static bool IsSuppressed(this CursorState cursorState)
    {
        return cursorState == CursorState.Suppressed;
    }
}