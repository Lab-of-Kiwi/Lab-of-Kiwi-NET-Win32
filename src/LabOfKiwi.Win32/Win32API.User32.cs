using LabOfKiwi.Win32.Native;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32;

public static partial class Win32API
{
    public static void DeleteMenu(HANDLE menu, SC item)
    {
        if (!User32.DeleteMenu(menu, (uint)item, (uint)MF.ByCommand))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static void DeleteMenu(HANDLE menu, int position)
    {
        if (!User32.DeleteMenu(menu, (uint)position, (uint)MF.ByPosition))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static HANDLE GetSystemMenu(HANDLE wnd)
    {
        return User32.GetSystemMenu(wnd, false);
    }

    public static void RevertSystemMenu(HANDLE wnd)
    {
        User32.GetSystemMenu(wnd, true);
    }
}
