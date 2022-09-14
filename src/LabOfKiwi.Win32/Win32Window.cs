using LabOfKiwi.Win32.Native;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32;

/// <summary>
/// Represents a Windows window object.
/// </summary>
public abstract class Win32Window : Win32Object
{
    protected volatile Win32Menu? _menu;

    internal Win32Window(nint handle) : base(handle)
    {
    }

    internal object SyncRoot => _syncRoot;

    /// <summary>
    /// Gets the <see cref="Win32Menu"/> for this instance.
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    public Win32Menu Menu
    {
        get
        {
            lock (_syncRoot)
            {
                ThrowIfDisposed();

                if (_menu == null)
                {
                    nint menuHandle = User32.GetSystemMenu(Handle, false);
                    _menu = new Win32Menu(menuHandle, this);
                }

                return _menu;
            }
        }
    }

    /// <summary>
    /// Redraws the menu of this window.
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    /// <exception cref="Win32Exception">A Win32 error occurs while redrawing this window's menu.</exception>
    public void RedrawMenuBar()
    {
        lock (_syncRoot)
        {
            ThrowIfDisposed();

            if (!User32.DrawMenuBar(Handle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }

    /// <summary>
    /// Reverts this window's menu to its original state.
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    public void RevertMenu()
    {
        lock (_syncRoot)
        {
            if (_menu != null)
            {
                lock (_menu.SyncRoot)
                {
                    _menu.ThrowIfDisposed();
                    User32.GetSystemMenu(Handle, true);
                }
            }
            else
            {
                ThrowIfDisposed();
                User32.GetSystemMenu(Handle, true);
            }
        }
    }
}
