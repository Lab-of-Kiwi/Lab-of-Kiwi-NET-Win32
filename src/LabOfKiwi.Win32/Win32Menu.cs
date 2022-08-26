using LabOfKiwi.Win32.Native;
using System;
using System.Collections.Generic;

namespace LabOfKiwi.Win32;

public sealed class Win32Menu : Win32Object
{
    private readonly Win32Window _parentWindow;

    internal Win32Menu(nint handle, Win32Window parentWindow) : base(handle)
    {
        _parentWindow = parentWindow;
    }

    public Win32Window ParentWindow
    {
        get
        {
            ThrowIfDisposed();
            return _parentWindow;
        }
    }

    public void DeleteItem(MenuItem item)
    {
        ThrowIfDisposed();
        User32.DeleteMenu(Handle, (uint)item, (uint)MF.ByCommand);
    }

    public void DeleteItems(params MenuItem[] items)
    {
        DeleteItems((IEnumerable<MenuItem>)items);
    }

    public void DeleteItems(IEnumerable<MenuItem> items)
    {
        ThrowIfDisposed();

        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        foreach (var item in items)
        {
            User32.DeleteMenu(Handle, (uint)item, (uint)MF.ByCommand);
        }
    }
}
