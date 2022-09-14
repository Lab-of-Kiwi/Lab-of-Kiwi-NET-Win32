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

    internal object SyncRoot => _syncRoot;

    public Win32Window ParentWindow
    {
        get
        {
            lock (_syncRoot)
            {
                ThrowIfDisposed();
                return _parentWindow;
            }
        }
    }

    public void DeleteItem(MenuItem item)
    {
        lock (_syncRoot)
        {
            lock (_parentWindow.SyncRoot)
            {
                ThrowIfDisposed();
                User32.DeleteMenu(Handle, (uint)item, (uint)MF.ByCommand);
                // Note: would check for Win32 error, but for some reason some menu items returns false
                // but are still deleted.
            }
        }
    }

    public void DeleteItems(params MenuItem[] items)
    {
        DeleteItems((IEnumerable<MenuItem>)items);
    }

    public void DeleteItems(IEnumerable<MenuItem> items)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        lock (_syncRoot)
        {
            lock (_parentWindow.SyncRoot)
            {
                ThrowIfDisposed();

                foreach (var item in items)
                {
                    User32.DeleteMenu(Handle, (uint)item, (uint)MF.ByCommand);
                }
            }
        }
    }

    public void Revert()
    {
        lock (_syncRoot)
        {
            lock (_parentWindow.SyncRoot)
            {
                ThrowIfDisposed();
                User32.GetSystemMenu(_parentWindow.Handle, true);
            }
        }
    }

    internal override void ThrowIfDisposed()
    {
        base.ThrowIfDisposed();
        _parentWindow.ThrowIfDisposed();
    }
}
