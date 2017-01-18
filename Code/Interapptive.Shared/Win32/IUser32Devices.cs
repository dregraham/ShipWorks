using System;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for managing devices
    /// </summary>
    public interface IUser32Devices
    {
        /// <summary>
        /// Get information about a device
        /// </summary>
        string GetDeviceName(IntPtr device);

        /// <summary>
        /// Register raw input device
        /// </summary>
        bool RegisterRawInputDevice(RawInputDevice device);
    }
}
