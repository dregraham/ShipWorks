using System;
using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Item in a raw input device list
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDeviceListItem
    {
        /// <summary>
        /// Handle of the device
        /// </summary>
        public IntPtr DeviceHandle;

        /// <summary>
        /// Type of raw input device
        /// </summary>
        public RawInputDeviceType Type;
    }
}
