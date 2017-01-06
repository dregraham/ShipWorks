using System;
using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Value type for a raw input header.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputHeader
    {
        /// <summary>
        /// Type of device the input is coming from.
        /// </summary>
        public RawInputDeviceType Type;

        /// <summary>
        /// Size of the packet of data.
        /// </summary>
        public int Size;

        /// <summary>
        /// Handle to the device sending the data.
        /// </summary>
        public IntPtr DeviceHandle;

        /// <summary>
        /// wParam from the window message.
        /// </summary>
        public IntPtr WParam;
    }
}