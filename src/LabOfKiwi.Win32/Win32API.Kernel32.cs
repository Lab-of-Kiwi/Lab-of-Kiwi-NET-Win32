using LabOfKiwi.Win32.Native;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32;

public static partial class Win32API
{
    public static void CloseHandle(HANDLE obj)
    {
        if (!Kernel32.CloseHandle(obj))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static HANDLE CreateConsoleScreenBuffer(GENERIC desiredAccess, FILE_SHARE shareMode)
    {
        HANDLE handle = Kernel32.CreateConsoleScreenBuffer((uint)desiredAccess, (uint)shareMode, 0, (uint)CONSOLE_TEXTMODE.Buffer, 0);

        if (handle.IsInvalid)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        return handle;
    }

    public static CONSOLE_CURSOR_INFO GetConsoleCursorInfo(HANDLE consoleOutput)
    {
        if (!Kernel32.GetConsoleCursorInfo(consoleOutput, out CONSOLE_CURSOR_INFO consoleCursorInfo))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        return consoleCursorInfo;
    }

    public static HANDLE GetConsoleWindow()
    {
        return Kernel32.GetConsoleWindow();
    }

    public static void SetConsoleActiveScreenBuffer(HANDLE consoleOutput)
    {
        if (!Kernel32.SetConsoleActiveScreenBuffer(consoleOutput))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static void SetConsoleCursorInfo(HANDLE consoleOutput, CONSOLE_CURSOR_INFO consoleCursorInfo)
    {
        if (!Kernel32.SetConsoleCursorInfo(consoleOutput, ref consoleCursorInfo))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static void SetConsoleCursorPosition(HANDLE consoleOutput, short xPosition, short yPosition)
    {
        COORD cursorPosition = new()
        {
            X = xPosition,
            Y = yPosition
        };

        if (!Kernel32.SetConsoleCursorPosition(consoleOutput, cursorPosition))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static int WriteConsoleOutputCharacter(HANDLE consoleOutput, string text, int length, short xWriteCoord = 0, short yWriteCoord = 0)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        COORD writeCoord = new()
        {
            X = xWriteCoord,
            Y = yWriteCoord
        };

        if (!Kernel32.WriteConsoleOutputCharacter(consoleOutput, text, (uint)length, writeCoord, out uint numCharsWritten))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        return (int)numCharsWritten;
    }

    public static int WriteConsoleOutputCharacter(HANDLE consoleOutput, string text, short xWriteCoord = 0, short yWriteCoord = 0)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        return WriteConsoleOutputCharacter(consoleOutput, text, text.Length, xWriteCoord, yWriteCoord);
    }

    public static unsafe int WriteConsoleOutputCharacter(HANDLE consoleOutput, Span<char> text, int length, short xWriteCoord = 0, short yWriteCoord = 0)
    {
        uint numCharsWritten;

        COORD writeCoord = new()
        {
            X = xWriteCoord,
            Y = yWriteCoord
        };

        fixed (char* textPtr = &text[0])
        {
            if (!Kernel32.WriteConsoleOutputCharacter(consoleOutput, textPtr, (uint)length, writeCoord, out numCharsWritten))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        return (int)numCharsWritten;
    }

    public static int WriteConsoleOutputCharacter(HANDLE consoleOutput, Span<char> text, short xWriteCoord = 0, short yWriteCoord = 0)
    {
        return WriteConsoleOutputCharacter(consoleOutput, text, text.Length, xWriteCoord, yWriteCoord);
    }
}
