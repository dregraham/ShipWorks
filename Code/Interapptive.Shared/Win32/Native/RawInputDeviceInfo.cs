using System.Runtime.InteropServices;

namespace Interapptive.Shared.Win32.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RawInputDeviceInfo
    {
        /// <summary>
        /// The size, in bytes, of the RID_DEVICE_INFO structure.
        /// </summary>
        [FieldOffset(0)]
        public int cbSize;

        /// <summary>
        /// The type of raw input data.
        /// </summary>
        [FieldOffset(4)]
        public RawInputDeviceType dwType;

        /// <summary>
        /// If dwType is RIM_TYPEMOUSE, this is the RID_DEVICE_INFO_MOUSE structure that defines the mouse.
        /// </summary>
        [FieldOffset(8)]
        public RawInputDeviceMouseInfo mouse;

        /// <summary>
        /// If dwType is RIM_TYPEKEYBOARD, this is the RID_DEVICE_INFO_KEYBOARD structure that defines the keyboard.
        /// </summary>
        [FieldOffset(8)]
        public RawInputDeviceKeyboardInfo keyboard;

        /// <summary>
        /// If dwType is RIM_TYPEHID, this is the RID_DEVICE_INFO_HID structure that defines the HID device.
        /// </summary>
        [FieldOffset(8)]
        public RawInputDeviceHidInfo hid;
    }
}
