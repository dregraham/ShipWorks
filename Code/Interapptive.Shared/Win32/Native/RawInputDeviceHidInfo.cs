using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Defines the raw input data coming from the specified Human Interface Device (HID).
    /// </summary>
    /// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/ms645584%28v=vs.85%29.aspx</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDeviceHidInfo
    {
        /// <summary>
        /// The vendor identifier for the HID.
        /// </summary>
        public int VendorId;

        /// <summary>
        /// The product identifier for the HID.
        /// </summary>
        public int ProductId;

        /// <summary>
        /// The version number for the HID.
        /// </summary>
        public int VersionNumber;

        /// <summary>
        /// The top-level collection Usage Page for the device.
        /// </summary>
        public ushort UsagePage;

        /// <summary>
        /// The top-level collection Usage for the device.
        /// </summary>
        public ushort Usage;
    }
}