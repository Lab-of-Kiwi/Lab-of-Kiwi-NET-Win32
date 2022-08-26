using LabOfKiwi.Win32.Native;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LabOfKiwi.Win32;

/// <summary>
/// A console screen buffer that can be attached to the console.
/// </summary>
public sealed class Win32ConsoleScreenBuffer : Win32Object, IDisposable
{
    // The value of the pointer returned if a screen buffer is not able to be created.
    private const nint InvalidHandleValue = -1;

    // Internal constructor.
    private Win32ConsoleScreenBuffer(nint handle) : base(handle)
    {
    }

    // Finalizer.
    ~Win32ConsoleScreenBuffer()
    {
        DisposeResources();
    }

    /// <summary>
    /// The size of the cursor. If the cursor is not visible, a value of 0 is returned; otherwise, a value between 1 and
    /// 100 is returned (unless not set previously and the system has a non-standard value set).
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    /// <exception cref="Win32Exception">A Win32 error occurs while retrieving or setting the cursor size.</exception>
    public int CursorSize
    {
        get
        {
            CONSOLE_CURSOR_INFO cursorInfo;

            lock (_syncRoot)
            {
                ThrowIfDisposed();

                if (!Kernel32.GetConsoleCursorInfo(Handle, out cursorInfo))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }


            return cursorInfo.Visible ? (int)cursorInfo.Size : 0;
        }

        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            CONSOLE_CURSOR_INFO cursorInfo;

            if (value == 0)
            {
                cursorInfo = new() { Size = 25U, Visible = false };
            }
            else
            {
                cursorInfo = new() { Size = (uint)value, Visible = true };
            }

            lock (_syncRoot)
            {
                ThrowIfDisposed();

                if (!Kernel32.SetConsoleCursorInfo(Handle, ref cursorInfo))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
    }

    /// <summary>
    /// Creates a new <see cref="Win32ConsoleScreenBuffer"/>. This instance is not active until <see cref="SetActive"/>
    /// is called.
    /// </summary>
    /// 
    /// <returns>The new <see cref="Win32ConsoleScreenBuffer"/> in an inactive state.</returns>
    /// 
    /// <exception cref="Win32Exception">A Win32 error occurs while creating the new instance.</exception>
    public static Win32ConsoleScreenBuffer Create()
    {
        nint handle = Kernel32.CreateConsoleScreenBuffer((uint)GENERIC.ReadWrite, (uint)FILE_SHARE.ReadWrite, 0, (uint)CONSOLE_TEXTMODE.Buffer, 0);

        if (handle == InvalidHandleValue)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        return new Win32ConsoleScreenBuffer(handle);
    }

    /// <summary>
    /// Disposes this instance by calling the <c>CloseHandle</c> method in the Kernel32.dll.
    /// </summary>
    public void Dispose()
    {
        lock (_syncRoot)
        {
            lock (Win32ConsoleWindow.Instance.SyncRoot)
            {
                DisposeResources();
                Win32ConsoleWindow.Instance.RemoveIfSame(this);
            }
        }
        
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sets this instance as the active <see cref="Win32ConsoleScreenBuffer"/>.
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    /// <exception cref="Win32Exception">A Win32 error occurs when setting this instance as active.</exception>
    public void SetActive()
    {
        lock (_syncRoot)
        {
            ThrowIfDisposed();

            lock (Win32ConsoleWindow.Instance.SyncRoot)
            {
                if (!Kernel32.SetConsoleActiveScreenBuffer(Handle))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                Win32ConsoleWindow.Instance.ActiveScreenBuffer = this;
            }
        }
    }

    /// <summary>
    /// Writes the provided <see cref="string"/> to this instance at the optionally provided <see cref="Point"/>
    /// coordinate.
    /// </summary>
    /// 
    /// <param name="text">The text to write to this instance.</param>
    /// <param name="position">The starting position in this instance to start writing.</param>
    /// <returns>The number of characters written.</returns>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The <c>X</c> or <Y> coordinate of <paramref name="position"/></Y> is less than <see cref="short.MinValue"/>
    ///     or greater than <see cref="short.MaxValue"/>.
    /// </exception>
    /// <exception cref="Win32Exception">A Win32 error occurs writing characters to this instance.</exception>
    public int Write(string? text, Point position = default)
    {
        if (position.X < short.MinValue || position.X > short.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        if (position.Y < short.MinValue || position.Y > short.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        if (text != null && text.Length > 0)
        {
            COORD writeCoord = new() { X = (short)position.X, Y = (short)position.Y };

            uint charsWritten;
            bool success;

            lock (_syncRoot)
            {
                ThrowIfDisposed();
                success = Kernel32.WriteConsoleOutputCharacter(Handle, text, (uint)text.Length, writeCoord, out charsWritten);
            }

            if (!success)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return (int)charsWritten;
        }

        return 0;
    }

    /// <summary>
    /// Writes a range of characters in the provided byte array to this instance at the optionally provided
    /// <see cref="Point"/> coordinate.
    /// </summary>
    /// 
    /// <param name="text">The text to write to this instance.</param>
    /// <param name="startIndex">The starting index within <paramref name="text"/> to start.</param>
    /// <param name="count">The number of characters to write.</param>
    /// <param name="position">The starting position in this instance to start writing.</param>
    /// <returns>The number of characters written.</returns>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The <c>X</c> or <Y> coordinate of <paramref name="position"/></Y> is less than <see cref="short.MinValue"/>
    ///     or greater than <see cref="short.MaxValue"/>.
    ///     
    ///     -or-
    ///     
    ///     <paramref name="startIndex"/> and/or <paramref name="count"/> is invalid for the range in
    ///     <paramref name="text"/>.
    /// </exception>
    /// <exception cref="Win32Exception">A Win32 error occurs writing characters to this instance.</exception>
    public int Write(char[]? text, int startIndex, int count, Point position = default)
    {
        ReadOnlySpan<char> span = text;
        span = span.Slice(startIndex, count);
        return Write(span, position);
    }

    /// <summary>
    /// Writes the characters in the provided range of memory to this instance at the optionally provided
    /// <see cref="Point"/> coordinate.
    /// </summary>
    /// 
    /// <param name="text">The text to write to this instance.</param>
    /// <param name="position">The starting position in this instance to start writing.</param>
    /// <returns>The number of characters written.</returns>
    /// 
    /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The <c>X</c> or <Y> coordinate of <paramref name="position"/></Y> is less than <see cref="short.MinValue"/>
    ///     or greater than <see cref="short.MaxValue"/>.
    /// </exception>
    /// <exception cref="Win32Exception">A Win32 error occurs writing characters to this instance.</exception>
    public unsafe int Write(ReadOnlySpan<char> text, Point position = default)
    {
        if (position.X < short.MinValue || position.X > short.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        if (position.Y < short.MinValue || position.Y > short.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        if (text.Length > 0)
        {
            COORD writeCoord = new() { X = (short)position.X, Y = (short)position.Y };

            uint charsWritten;
            bool success;

            lock (_syncRoot)
            {
                ThrowIfDisposed();

                fixed (char* ptr = &text[0])
                {
                    success = Kernel32.WriteConsoleOutputCharacter(Handle, ptr, (uint)text.Length, writeCoord, out charsWritten);
                }
            }

            if (!success)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return (int)charsWritten;
        }

        return 0;
    }

    private void DisposeResources()
    {
        if (!_disposedValue)
        {
            CloseHandle();
            _disposedValue = true;
        }
    }
}
