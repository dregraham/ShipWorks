using System;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for managing devices
    /// </summary>
    public class User32Devices : IUser32Devices
    {
        /// <summary>
        /// Get a list of all raw devices
        /// </summary>
        public RawInputDeviceListItem[] GetAllRawDevices()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get information about a device
        /// </summary>
        public RawInputDeviceInfo GetDeviceInfo(RawInputDeviceListItem device)
        {
            throw new NotImplementedException();
        }
    }
}
