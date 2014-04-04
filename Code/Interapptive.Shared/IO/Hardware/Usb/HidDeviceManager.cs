using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using log4net;
using Interapptive.Shared.Business;

namespace Interapptive.Shared.Usb
{
    /// <summary>
    /// Entry point into finding and managing HID devices in the system
    /// </summary>
    public static class HidDeviceManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(HidDeviceManager));

        static Guid hidGuid = Guid.Empty;

        /// <summary>
        /// Static constructor
        /// </summary>
        static HidDeviceManager()
        {
            NativeUsb.HidD_GetHidGuid(out hidGuid);
        }

        /// <summary>
        /// Find all HidDevices in the system
        /// </summary>
        public static List<HidDevice> FindDevices()
        {
            List<HidDevice> devices = new List<HidDevice>();

            IntPtr deviceInfoSet = NativeUsb.SetupDiGetClassDevs(ref hidGuid, null, IntPtr.Zero, NativeUsb.DIGCF_DEVICEINTERFACE | NativeUsb.DIGCF_PRESENT);

            NativeUsb.DeviceInterfaceData diData = new NativeUsb.DeviceInterfaceData();
            diData.Size = Marshal.SizeOf(diData);

            int memberIndex = 0;
            while (NativeUsb.SetupDiEnumDeviceInterfaces(deviceInfoSet, 0, ref hidGuid, memberIndex, ref diData))
            {
                string devicePath = GetDevicePath(deviceInfoSet, diData);

                if (devicePath != null)
                {
                    try
                    {
                        HidDevice device = HidDevice.Initialize(devicePath);
                        devices.Add(device);

                        // log.InfoFormat("Loaded HID {0} {1}", device.Attributes.VendorHexID, device.Attributes.ProductHexID);
                    }
                    catch (HidException ex)
                    {
                        log.Warn("Could not load HID Device " + devicePath, ex);
                    }
                }

                memberIndex++;
            }

            NativeUsb.SetupDiDestroyDeviceInfoList(deviceInfoSet);

            return devices;
        }

        /// <summary>
        /// Try to get the DevicePath for the given device interface data
        /// </summary>
        private static string GetDevicePath(IntPtr deviceInfoSet, NativeUsb.DeviceInterfaceData diData)
        {
            int requiredSize = 0;

            // Get the device interface details
            if (!NativeUsb.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref diData, IntPtr.Zero, 0, ref requiredSize, IntPtr.Zero))
            {
                NativeUsb.DeviceInterfaceDetailData diDetail = new NativeUsb.DeviceInterfaceDetailData();

                // Bit of magic here.  Not sure why these numbers work - but they are the only ones that work!
                diDetail.Size = MyComputer.Is64BitProcess ? 8 : 5;

                if (NativeUsb.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref diData, ref diDetail, requiredSize, ref requiredSize, IntPtr.Zero))
                {
                    return diDetail.DevicePath;
                }
            }

            return null;
        }

        /// <summary>
        /// Registers a window to receive windows messages when a device is inserted/removed. Need to call this
        /// from a form when its handle has been created, not in the form constructor. Use form's OnHandleCreated override.
        /// </summary>
        /// <param name="hWnd">Handle to window that will receive messages</param>
        /// <param name="gClass">Class of devices to get messages for</param>
        /// <returns>A handle used when unregistering</returns>
        public static IntPtr RegisterForUsbEvents(IntPtr hWnd, Guid gClass)
        {
            NativeUsb.DeviceBroadcastInterface oInterfaceIn = new NativeUsb.DeviceBroadcastInterface();
            oInterfaceIn.Size = Marshal.SizeOf(oInterfaceIn);
            oInterfaceIn.ClassGuid = gClass;
            oInterfaceIn.DeviceType = NativeUsb.DEVTYP_DEVICEINTERFACE;
            oInterfaceIn.Reserved = 0;
            return NativeUsb.RegisterDeviceNotification(hWnd, oInterfaceIn, NativeUsb.DEVICE_NOTIFY_WINDOW_HANDLE);
        }

        /// <summary>
        /// Unregisters notifications. Can be used in form dispose
        /// </summary>
        /// <param name="hHandle">Handle returned from RegisterForUSBEvents</param>
        /// <returns>True if successful</returns>
        public static bool UnregisterForUsbEvents(IntPtr hHandle)
        {
            return NativeUsb.UnregisterDeviceNotification(hHandle);
        }

        /// <summary>
        /// Helper to get the HID guid.
        /// </summary>
        public static Guid HidGuid
        {
            get
            {
                return hidGuid;
            }
        }
    }
}
