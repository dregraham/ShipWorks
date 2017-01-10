using System;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for managing devices
    /// </summary>
    [CLSCompliant(false)]
    public class User32Devices : IUser32Devices
    {
        /// <summary>
        /// Get a list of all raw devices
        /// </summary>
        public RawInputDeviceListItem[] GetAllRawDevices()
        {
            //throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// Get information about a device
        /// </summary>
        public RawInputDeviceInfo GetDeviceInfo(RawInputDeviceListItem device)
        {
            //throw new NotImplementedException();
            return new RawInputDeviceInfo();
        }

        /// <summary>
        /// Register raw input device
        /// </summary>
        public bool RegisterRawInputDevice(RawInputDevice device)
        {
            //throw new NotImplementedException();
            return false;
        }
    }
}
