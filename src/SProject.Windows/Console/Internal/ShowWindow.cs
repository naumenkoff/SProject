namespace SProject.Windows.Console.Internal;

/// <summary>
///     https://learn.microsoft.com/ru-ru/windows/win32/api/winuser/nf-winuser-showwindow
/// </summary>
internal enum ShowWindow
{
    /// <summary>
    ///     Hides the window and activates another window.
    /// </summary>
    Hide = 0,

    /// <summary>
    ///     Activates the window and displays it in its current size and position.
    /// </summary>
    Show = 5
}