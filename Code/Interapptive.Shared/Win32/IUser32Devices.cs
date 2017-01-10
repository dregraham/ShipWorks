using System;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for managing devices
    /// </summary>
    [CLSCompliant(false)]
    public interface IUser32Devices
    {
        /// <summary>
        /// Get a list of all raw devices
        /// </summary>
        RawInputDeviceListItem[] GetAllRawDevices();

        /// <summary>
        /// Get information about a device
        /// </summary>
        RawInputDeviceInfo GetDeviceInfo(RawInputDeviceListItem device);

        /// <summary>
        /// Register raw input device
        /// </summary>
        bool RegisterRawInputDevice(RawInputDevice device);
    }
}
