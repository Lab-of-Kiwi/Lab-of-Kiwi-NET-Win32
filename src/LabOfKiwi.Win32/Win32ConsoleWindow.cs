using LabOfKiwi.Win32.Native;

namespace LabOfKiwi.Win32;

/// <summary>
/// Represents this process's Win32 console window.
/// </summary>
public sealed class Win32ConsoleWindow : Win32Window
{
    /// <summary>
    /// Gets the instance of this process's console window.
    /// </summary>
    public static Win32ConsoleWindow Instance { get; }

    // Static initializer.
    static Win32ConsoleWindow()
    {
        nint handle = Kernel32.GetConsoleWindow();
        Instance = new(handle);
    }

    private volatile Win32ConsoleScreenBuffer? _activeScreenBuffer;

    private Win32ConsoleWindow(nint handle) : base(handle)
    {
    }

    internal object SyncRoot => _syncRoot;

    /// <summary>
    /// Gets the active screen buffer, if set; otherwise, <c>null</c>.
    /// </summary>
    public Win32ConsoleScreenBuffer? ActiveScreenBuffer
    {
        get
        {
            lock (_syncRoot)
            {
                return _activeScreenBuffer;
            }
        }

        internal set
        {
            _activeScreenBuffer = value;
        }
    }

    internal void RemoveIfSame(Win32ConsoleScreenBuffer screenBuffer)
    {
        if (screenBuffer.Equals(_activeScreenBuffer))
        {
            _activeScreenBuffer = null;
        }
    }
}
