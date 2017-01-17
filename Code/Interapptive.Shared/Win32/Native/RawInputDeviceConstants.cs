using System;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Constants for dealing with raw input devices
    /// </summary>
    /// <remarks>
    /// https://msdn.microsoft.com/en-us/windows/hardware/drivers/hid/hid-clients-supported-in-windows
    /// </remarks>
    [CLSCompliant(false)]
    public static class RawInputDeviceConstants
    {
        /// <summary>
        /// Keyboard constants
        /// </summary>
        public struct Keyboard
        {
            /// <summary>
            /// Usage page value for keyboards
            /// </summary>
            public static ushort UsagePage => 0x01;

            /// <summary>
            /// Usage value for keyboards
            /// </summary>
            public static ushort Usage => 0x06;
        }
    }
}