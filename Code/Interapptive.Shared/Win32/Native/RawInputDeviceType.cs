namespace Interapptive.Shared.Win32.Native
{
    /// <summary>
    /// Types of raw input devices
    /// </summary>
    public enum RawInputDeviceType : uint
    {
        /// <summary>
        /// Device is a mouse
        /// </summary>
        Mouse = 0,

        /// <summary>
        /// Device is a keyboard
        /// </summary>
        Keyboard = 1,

        /// <summary>
        /// Device is HID
        /// </summary>
        Hid = 2
    }
}
