using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Interapptive.Shared.Win32.Native;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// User32 wrapper for managing devices
    /// </summary>
    [CLSCompliant(false)]
    public class User32Devices : IUser32Devices
    {
        /// <summary>Function to register a raw input device.</summary>
        /// <param name="pRawInputDevices">Array of raw input devices.</param>
        /// <param name="uiNumDevices">Number of devices.</param>
        /// <param name="cbSize">Size of the RAWINPUTDEVICE structure.</param>
        /// <returns>TRUE if successful, FALSE if not.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterRawInputDevices(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RawInputDevice[] pRawInputDevices,
            int uiNumDevices,
            int cbSize);

        /// <summary>
        /// Function to get a raw input device
        /// </summary>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetRawInputDeviceList
        (
            [In, Out] RawInputDeviceListItem[] RawInputDeviceList,
            ref uint NumDevices,
            uint Size 
        );
		
		/// <summary>
        /// Retrieves information about the raw input device.
        /// </summary>
        /// <param name="hDevice">A handle to the raw input device. This comes from the lParam of the WM_INPUT message, from the hDevice member of RAWINPUTHEADER, or from GetRawInputDeviceList. It can also be NULL if an application inserts input data, for example, by using SendInput.</param>
        /// <param name="uiCommand">Specifies what data will be returned in pData.</param>
        /// <param name="pData">A pointer to a buffer that contains the information specified by uiCommand. If uiCommand is RIDI_DEVICEINFO, set the cbSize member of RID_DEVICE_INFO to sizeof(RID_DEVICE_INFO) before calling GetRawInputDeviceInfo. </param>
        /// <param name="pcbSize">The size, in bytes, of the data in pData. </param>
        /// <returns>If successful, this function returns a non-negative number indicating the number of bytes copied to pData. If pData is not large enough for the data, the function returns -1. If pData is NULL, the function returns a value of zero. In both of these cases, pcbSize is set to the minimum size required for the pData buffer. Call GetLastError to identify any other errors.</returns>
        /// <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/ms645597%28v=vs.85%29.aspx</remarks>
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetRawInputDeviceInfo(
            [In] IntPtr hDevice,
            [In] RawInputDeviceInformationCommand uiCommand,
            [In, Out] IntPtr pData,
            [In, Out] ref uint pcbSize);

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string GetDeviceName(IntPtr deviceHandle)
        {
            IntPtr pData = IntPtr.Zero;
            uint size = 0;

            // These are the pinvoke calls needed to get the hid path.
            GetRawInputDeviceInfo(deviceHandle, RawInputDeviceInformationCommand.DeviceName, pData, ref size);
            pData = Marshal.AllocHGlobal(((int) size) * 2);
            GetRawInputDeviceInfo(deviceHandle, RawInputDeviceInformationCommand.DeviceName, pData, ref size);

            // hidPAth will be a string like \\?\blahblah#VID_12A0&PID_A73B#lkjasdf#lkjsdlfk
            // We want to capture VID_12A0&PID_A73B... 
            // VID represents the vendor ID and PID is the productID.
            string hidPath = Marshal.PtrToStringAuto(pData);
            string[] hidParts = hidPath?.Split('#');
            int length = hidParts?.Length ?? 0;

            return length >= 2 ? hidParts[1] : null;
        }

        /// <summary>
        /// Register raw input device
        /// </summary>
        public bool RegisterRawInputDevice(RawInputDevice device)
        {
            bool result = RegisterRawInputDevices(new[] { device }, 1, Marshal.SizeOf(typeof(RawInputDevice)));
            return result;
        }
    }
}
