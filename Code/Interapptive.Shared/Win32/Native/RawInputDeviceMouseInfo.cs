using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Defines the raw input data coming from the specified mouse.
    /// </summary>
    /// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/ms645589%28v=vs.85%29.aspx</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDeviceMouseInfo
    {
        /// <summary>
        /// The identifier of the mouse device.
        /// </summary>
        public int Id;

        /// <summary>
        /// The number of buttons for the mouse.
        /// </summary>
        public int NumberOfButtons;

        /// <summary>
        /// The number of data points per second. This information may not be applicable for every mouse device.
        /// </summary>
        public int SampleRate;

        /// <summary>
        /// TRUE if the mouse has a wheel for horizontal scrolling; otherwise, FALSE.
        /// Windows XP:  This member is only supported starting with Windows Vista.
        /// </summary>
        public bool HasHorizontalWheel;
    }
}