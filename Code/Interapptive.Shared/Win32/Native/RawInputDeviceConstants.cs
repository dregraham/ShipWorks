using System;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Constants for dealing with raw input devices
    /// https://msdn.microsoft.com/en-us/windows/hardware/drivers/hid/hid-clients-supported-in-windows
    /// </summary>
    [CLSCompliant(false)]
    public static class RawInputDeviceConstants
    {
        /// <summary>
        /// Keyboard constants
        /// </summary>
        public struct Keyboard
        {
            public static ushort UsagePage => 0x01;

            public static ushort Usage => 0x06;
        }
    }
}