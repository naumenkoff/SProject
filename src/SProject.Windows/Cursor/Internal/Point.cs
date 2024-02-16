using System.Runtime.InteropServices;

namespace SProject.Windows.Cursor.Internal;

/// <summary>
///     https://learn.microsoft.com/ru-ru/windows/win32/api/windef/ns-windef-point
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    /// <summary>
    ///     Specifies the x-coordinate of the point.
    /// </summary>
    public int x;

    /// <summary>
    ///     Specifies the y-coordinate of the point.
    /// </summary>
    public int y;
}