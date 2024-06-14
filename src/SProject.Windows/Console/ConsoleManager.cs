using SProject.Windows.Console.Internal;

namespace SProject.Windows.Console;

// CA1822: Mark members as static
#pragma warning disable CA1822

public sealed class ConsoleManager : IDisposable
{
    private static StreamWriter? _stdErr;
    private static StreamWriter? _stdOut;

    public bool HasExternalConsole { get; } = ConsoleInvoke.AttachConsole(0x0FFFFFFFF);

    public void Dispose()
    {
        _stdErr?.Dispose();
        _stdOut?.Dispose();
    }

    public void Initialize()
    {
        if (HasExternalConsole || HasConsoleWindow(out _)) return;

        ConsoleInvoke.AllocConsole();
        HideConsole();
        RedirectConsoleOutput();
    }

    public void ShowConsole()
    {
        if (HasConsoleWindow(out var handle)) 
            ConsoleInvoke.ShowWindow(handle, (int)ShowWindow.Show);
    }

    public void HideConsole()
    {
        if (HasConsoleWindow(out var handle)) 
            ConsoleInvoke.ShowWindow(handle, (int)ShowWindow.Hide);
    }

    private static void RedirectConsoleOutput()
    {
        _stdOut = new StreamWriter(System.Console.OpenStandardError())
        {
            AutoFlush = true
        };
        _stdErr = new StreamWriter(System.Console.OpenStandardOutput())
        {
            AutoFlush = true
        };

        System.Console.SetOut(_stdOut);
        System.Console.SetError(_stdErr);
    }

    private static bool HasConsoleWindow(out IntPtr handle)
    {
        handle = ConsoleInvoke.GetConsoleWindow();
        return handle != IntPtr.Zero;
    }
}