using System.Runtime.InteropServices;

namespace SProject.Windows.Console.Internal;

internal static partial class ConsoleInvoke
{
    [LibraryImport("user32.dll")] internal static partial void ShowWindow(nint hWnd, int nCmdShow);

    [LibraryImport("kernel32.dll")] internal static partial void AllocConsole();

    [LibraryImport("kernel32.dll")] internal static partial IntPtr GetConsoleWindow();

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool AttachConsole(uint dwProcessId);
}