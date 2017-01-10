using System;
using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Contains the raw input from a device.
    /// </summary>
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInput
    {
        /// <summary>
        /// Header for the data.
        /// </summary>
        public RawInputHeader Header;

        public Union Data;

        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            /// <summary>
            /// Keyboard raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawInputKeyboard Keyboard;
        }
    }
}
