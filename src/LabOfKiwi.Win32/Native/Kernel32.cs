using System.Runtime.InteropServices;
using System.Text;

namespace LabOfKiwi.Win32.Native;

// Interop Methods Contained in Kernel32.dll
internal static class Kernel32
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(nint hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nint CreateConsoleScreenBuffer(
        uint dwDesiredAccess, uint dwShareMode, nint lpSecurityAttributes, uint dwFlags, nint lpScreenBufferData
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleCursorInfo(nint hConsoleOutput, out CONSOLE_CURSOR_INFO lpConsoleCursorInfo);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetConsoleTitle([Out] StringBuilder lpConsoleTitle, uint nSize);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nint GetConsoleWindow();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleActiveScreenBuffer(nint hConsoleOutput);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleCursorInfo(nint hConsoleOutput, [In] ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleCursorPosition(nint hConsoleOutput, COORD dwCursorPosition);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleTitle(string lpConsoleTitle);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static unsafe extern bool WriteConsoleOutputCharacter(
        nint hConsoleOutput, char* lpCharacter, uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteConsoleOutputCharacter(
        nint hConsoleOutput, string lpCharacter, uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten
    );
}
