using System;
using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Value type for raw input from a keyboard.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [CLSCompliant(false)]
    public struct RawInputKeyboard
    {
        /// <summary>Scan code for key depression.</summary>
        public short MakeCode;

        /// <summary>Scan code information.</summary>
        public RawKeyboardFlags Flags;

        /// <summary>Reserved.</summary>
        public short Reserved;

        /// <summary>Virtual key code.</summary>
        public VirtualKeys VirtualKey;

        /// <summary>Corresponding window message.</summary>
        public WindowsMessage Message;

        /// <summary>Extra information.</summary>
        public int ExtraInformation;
    }
}