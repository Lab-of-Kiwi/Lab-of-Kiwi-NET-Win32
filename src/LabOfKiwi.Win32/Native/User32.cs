using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32.Native;

// Interop Methods Contained in User32.dll
internal static class User32
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DeleteMenu(nint hMenu, uint uPosition, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DrawMenuBar(nint hMenu);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint GetSystemMenu(nint hWnd, bool bRevert);
}
