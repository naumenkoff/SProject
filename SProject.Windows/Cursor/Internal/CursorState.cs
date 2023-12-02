namespace SProject.Windows.Cursor.Internal;

public enum CursorState
{
    /// <summary>
    ///     The GetCursorInfo function failed
    /// </summary>
    Failed = -1,

    /// <summary>
    ///     The cursor is hidden.
    /// </summary>
    Hidden = 0,

    /// <summary>
    ///     The cursor is showing.
    /// </summary>
    Showing = 0x00000001,

    /// <summary>
    ///     Windows 8: The cursor is suppressed. This flag indicates that the system is not drawing the cursor because the user
    ///     is providing input through touch or pen instead of the mouse.
    /// </summary>
    Suppressed = 0x00000002
}