using System;
using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32;

[StructLayout(LayoutKind.Sequential)]
internal struct CONSOLE_CURSOR_INFO
{
    public uint Size;
    public bool Visible;
}

internal enum CONSOLE_TEXTMODE : uint
{
    Buffer = 0x00000001,
}

[StructLayout(LayoutKind.Sequential)]
internal struct COORD
{
    public short X;
    public short Y;
}

[Flags]
internal enum FILE_SHARE : uint
{
    None      = 0x00000000,
    Read      = 0x00000001,
    Write     = 0x00000002,
    ReadWrite = 0x00000003,
}

[Flags]
internal enum GENERIC : uint
{
    Read      = 0x80000000,
    Write     = 0x40000000,
    ReadWrite = 0xC0000000,
}

/// <summary>
/// Represents a window's menu item.
/// </summary>
public enum MenuItem : uint
{
    /// <summary>
    /// Size the window.
    /// </summary>
    Size = 0x0000F000,

    /// <summary>
    /// Move the window.
    /// </summary>
    Move = 0x0000F010,

    /// <summary>
    /// Maximize the window.
    /// </summary>
    Minimize = 0x0000F020,

    /// <summary>
    /// Minimize the window.
    /// </summary>
    Maximize = 0x0000F030,

    /// <summary>
    /// Move to the next window.
    /// </summary>
    NextWindow = 0x0000F040,

    /// <summary>
    /// Move to the previous window.
    /// </summary>
    PrevWindow = 0x0000F050,

    /// <summary>
    /// Close the window.
    /// </summary>
    Close = 0x0000F060,

    /// <summary>
    /// Scroll vertically.
    /// </summary>
    VScroll = 0x0000F070,

    /// <summary>
    /// Scroll horizontally.
    /// </summary>
    HScroll = 0x0000F080,

    /// <summary>
    /// Retrieve a menu through a mouse click.
    /// </summary>
    MouseMenu = 0x0000F090,

    /// <summary>
    /// Retrieve a menu through a keystroke.
    /// </summary>
    KeyMenu = 0x0000F100,

    Arrange = 0x0000F110,

    /// <summary>
    /// Save the previous coordinates (checkpoint).
    /// </summary>
    Restore = 0x0000F120,

    /// <summary>
    /// Activates the start menu.
    /// </summary>
    TaskList = 0x0000F130,

    /// <summary>
    /// Activates the start menu.
    /// </summary>
    ScreenSave = 0x0000F140,

    /// <summary>
    /// Activate the window associated with the application-specified hot key.
    /// </summary>
    HotKey = 0x0000F150,

    Default = 0x0000F160,

    /// <summary>
    /// Turn off the display.
    /// </summary>
    MonitorPower = 0x0000F170,

    /// <summary>
    /// Show help.
    /// </summary>
    ContextHelp = 0x0000F180,

    Separator = 0x0000F00F,
}

internal enum MF : uint
{
    ByCommand  = 0x00000000,
    ByPosition = 0x00000400,
}
