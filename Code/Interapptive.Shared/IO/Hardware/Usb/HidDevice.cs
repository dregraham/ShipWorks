using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Interapptive.Shared.Usb
{
    /// <summary>
    /// Represents a single USB HID device in the system
    /// </summary>
    public class HidDevice
    {
        string devicePath;

        HidDeviceAttributes attributes;
        HidDeviceCapabilities capabilities;

        /// <summary>
        /// Create a new instance of an HidDevice.  Throws HidException if the device could not be initialized
        /// </summary>
        public static HidDevice Initialize(string devicePath)
        {
            return new HidDevice(devicePath);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private HidDevice(string devicePath)
        {
            this.devicePath = devicePath;

            using (SafeFileHandle handle = OpenDevice(0))
            {
                attributes = ReadDeviceAttributes(handle);
                capabilities = ReadDeviceCapabilities(handle);
            }
        }

        /// <summary>
        /// The full HID device path of the device
        /// </summary>
        public string DevicePath
        {
            get { return devicePath; }
        }

        /// <summary>
        /// The vendor and product attributes of the device
        /// </summary>
        public HidDeviceAttributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// The capabilities of the device
        /// </summary>
        public HidDeviceCapabilities Capabilities
        {
            get { return capabilities; }
        }

        /// <summary>
        /// Synchronously read the input report from the device
        /// </summary>
        [SuppressMessage("SonarQube",
            "S2674: Check the return value of the \"Read\" call to see how many bytes were read.",
            Justification = "Number of bytes read at the end doesn't affect the results of the method")]
        public byte[] ReadInputReport()
        {
            try
            {
                using (SafeFileHandle handle = OpenDevice(NativeUsb.GENERIC_READ))
                {
                    using (FileStream stream = new FileStream(handle, FileAccess.Read, Capabilities.InputReportByteLength, true))
                    {
                        byte[] data = new byte[Capabilities.InputReportByteLength];
                        stream.Read(data, 0, data.Length);

                        return data;
                    }
                }
            }
            catch (Win32Exception ex)
            {
                throw new HidException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new HidException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Open the underlying device
        /// </summary>
        private SafeFileHandle OpenDevice(int accessMode)
        {
            SafeFileHandle handle = NativeUsb.CreateFile(devicePath,
                accessMode,
                NativeUsb.FILE_SHARE_READ | NativeUsb.FILE_SHARE_WRITE,
                IntPtr.Zero,
                NativeUsb.OPEN_EXISTING,
                NativeUsb.FILE_FLAG_OVERLAPPED,
                IntPtr.Zero);

            if (handle.IsClosed || handle.IsInvalid)
            {
                throw new HidException("Could not open USB device.", new Win32Exception());
            }

            return handle;
        }

        /// <summary>
        /// Read the device attributes of the specified device
        /// </summary>
        private HidDeviceAttributes ReadDeviceAttributes(SafeFileHandle handle)
        {
            NativeUsb.HidDAttributes hidd_attributes = new NativeUsb.HidDAttributes();
            hidd_attributes.Size = Marshal.SizeOf(hidd_attributes);

            NativeUsb.HidD_GetAttributes(handle, ref hidd_attributes);

            return new HidDeviceAttributes(hidd_attributes);
        }

        /// <summary>
        /// Read the capabilities of the specified device
        /// </summary>
        private HidDeviceCapabilities ReadDeviceCapabilities(SafeFileHandle handle)
        {
            NativeUsb.HidPCaps caps;
            IntPtr lpData;

            if (NativeUsb.HidD_GetPreparsedData(handle, out lpData))
            {
                NativeUsb.HidP_GetCaps(lpData, out caps);
                NativeUsb.HidD_FreePreparsedData(ref lpData);

                return new HidDeviceCapabilities(caps);
            }

            throw new HidException("Could not load device capabilities.", new Win32Exception());
        }
    }
}
