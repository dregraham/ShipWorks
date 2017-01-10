using System;
using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Raw input device that should have its input sent to ShipWorks
    /// </summary>
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDevice
    {
        /// <summary>
        /// Usage page
        /// </summary>
        public ushort UsagePage;

        /// <summary>
        /// Usage
        /// </summary>
        public ushort Usage;

        /// <summary>
        /// Flags
        /// </summary>
        public int Flags;

        /// <summary>
        /// Handle of the target for the messages
        /// </summary>
        public IntPtr TargetHandle;
    }
}
