using System;
using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct CONSOLE_CURSOR_INFO
{
    public uint Size;
    public bool Visible;
}

public enum CONSOLE_TEXTMODE : uint
{
    Buffer = 0x00000001,
}

[StructLayout(LayoutKind.Sequential)]
public struct COORD
{
    public short X;
    public short Y;
}

[Flags]
public enum FILE_SHARE : uint
{
    None      = 0x00000000,
    Read      = 0x00000001,
    Write     = 0x00000002,
    ReadWrite = 0x00000003,
}

[Flags]
public enum GENERIC : uint
{
    Read      = 0x80000000,
    Write     = 0x40000000,
    ReadWrite = 0xC0000000,
}

[StructLayout(LayoutKind.Sequential)]
public struct HANDLE
{
    private const nint InvalidHandleValue = -1;

    private readonly nint _handle;

    private HANDLE(nint handle)
    {
        _handle = handle;
    }

    public bool IsInvalid => _handle == InvalidHandleValue;

    public bool IsNull => _handle == 0;

    public static implicit operator HANDLE(nint value)
    {
        return new HANDLE(value);
    }

    public static implicit operator nint(HANDLE value)
    {
        return value._handle;
    }
}

public enum MF : uint
{
    ByCommand  = 0x00000000,
    ByPosition = 0x00000400,
}

public enum SC : uint
{
    Size         = 0x0000F000,
    Move         = 0x0000F010,
    Minimize     = 0x0000F020,
    Maximize     = 0x0000F030,
    NextWindow   = 0x0000F040,
    PrevWindow   = 0x0000F050,
    Close        = 0x0000F060,
    VScroll      = 0x0000F070,
    HScroll      = 0x0000F080,
    MouseMenu    = 0x0000F090,
    KeyMenu      = 0x0000F100,
    Arrange      = 0x0000F110,
    Restore      = 0x0000F120,
    TaskList     = 0x0000F130,
    ScreenSave   = 0x0000F140,
    HotKey       = 0x0000F150,
    Default      = 0x0000F160,
    MonitorPower = 0x0000F170,
    ContextHelp  = 0x0000F180,
    Separator    = 0x0000F00F,
}
