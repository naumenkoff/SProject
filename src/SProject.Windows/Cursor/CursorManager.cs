using SProject.Windows.Cursor.Internal;

namespace SProject.Windows.Cursor;

// CA1822: Mark members as static
#pragma warning disable CA1822

public class CursorManager
{
    public bool TryGetCursorInfo(out CursorInfo cursorInfo)
    {
        cursorInfo = CursorInfo.Empty;
        if (CursorInvoke.GetCursorInfo(ref cursorInfo)) return true;

        cursorInfo = default;
        return false;
    }

    public CursorState GetCursorState()
    {
        return TryGetCursorInfo(out var cursorInfo) ? (CursorState)cursorInfo.flags : CursorState.Failed;
    }
}