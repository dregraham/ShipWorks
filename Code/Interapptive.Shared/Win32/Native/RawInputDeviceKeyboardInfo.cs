using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Defines the raw input data coming from the specified keyboard.
    /// </summary>
    /// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/ms645587%28v=vs.85%29.aspx</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDeviceKeyboardInfo
    {
        /// <summary>
        /// The type of the keyboard.
        /// </summary>
        public int Type;

        /// <summary>
        /// The subtype of the keyboard.
        /// </summary>
        public int SubType;

        /// <summary>
        /// The scan code mode.
        /// </summary>
        public int KeyboardMode;

        /// <summary>
        /// The number of function keys on the keyboard.
        /// </summary>
        public int NumberOfFunctionKeys;

        /// <summary>
        /// The number of LED indicators on the keyboard.
        /// </summary>
        public int NumberOfIndicators;

        /// <summary>
        /// The total number of keys on the keyboard.
        /// </summary>
        public int NumberOfKeysTotal;
    }
}