using LabOfKiwi.Win32.Native;
using System;

namespace LabOfKiwi.Win32;

/// <summary>
/// Represents a Win32 object within the Windows API.
/// </summary>
public abstract class Win32Object
{
    protected readonly object _syncRoot = new();
    internal volatile bool _disposedValue;

    internal Win32Object(nint handle)
    {
        Handle = handle;
    }

    public sealed override bool Equals(object? obj)
    {
        return obj is Win32Object other && other.Handle == Handle;
    }

    public sealed override int GetHashCode()
    {
        return HashCode.Combine(nameof(Win32Object), Handle);
    }

    public sealed override string ToString()
    {
        string hex;

        if (Environment.Is64BitProcess)
        {
            hex = Convert.ToString(Handle, 16).PadLeft(16, '0');
        }
        else
        {
            hex = Convert.ToString((int)Handle, 16).PadLeft(8, '0');
        }

        return $"{base.ToString()}@{hex}";
    }

    protected nint Handle { get; }

    protected void CloseHandle()
    {
        Kernel32.CloseHandle(Handle);
    }

    protected void ThrowIfDisposed()
    {
        if (_disposedValue)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
