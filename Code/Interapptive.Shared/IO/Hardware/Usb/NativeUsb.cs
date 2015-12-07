using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Interapptive.Shared.Usb
{
    /// <summary>
    /// Native Win32 methods specifically for dealing with USB devices
    /// </summary>
    [NDependIgnore]
    public static class NativeUsb
    {
        #region Structures

        /// <summary>
        /// An overlapped structure used for overlapped IO operations. The structure is
        /// only used by the OS to keep state on pending operations. You don't need to fill anything in if you
        /// unless you want a Windows event to fire when the operation is complete.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Overlapped
        {
            public int Internal;
            public int InternalHigh;
            public int Offset;
            public int OffsetHigh;
            public IntPtr Event;
        }

        /// <summary>
        /// Provides details about a single USB device
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DeviceInterfaceData
        {
            public int Size;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        /// <summary>
        /// Provides the capabilities of a HID device
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HidPCaps
        {
            public short Usage;
            public short UsagePage;
            public short InputReportByteLength;
            public short OutputReportByteLength;
            public short FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public short[] Reserved;
            public short NumberLinkCollectionNodes;
            public short NumberInputButtonCaps;
            public short NumberInputValueCaps;
            public short NumberInputDataIndices;
            public short NumberOutputButtonCaps;
            public short NumberOutputValueCaps;
            public short NumberOutputDataIndices;
            public short NumberFeatureButtonCaps;
            public short NumberFeatureValueCaps;
            public short NumberFeatureDataIndices;
        }

        /// <summary>
        /// Access to the path for a device
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DeviceInterfaceDetailData
        {
            public int Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        /// <summary>
        /// Used when registering a window to receive messages about devices added or removed from the system.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public class DeviceBroadcastInterface
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public Guid ClassGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HidDAttributes
        {
            public int Size;
            public int VendorID;
            public int ProductID;
            public short VersionNumber;
        }

        #endregion

        #region Constants
        /// <summary>Windows message sent when a device is inserted or removed</summary>
        public const int WM_DEVICECHANGE = 0x0219;
        /// <summary>WParam for above : A device was inserted</summary>
        public const int DEVICE_ARRIVAL = 0x8000;
        /// <summary>WParam for above : A device was removed</summary>
        public const int DEVICE_REMOVECOMPLETE = 0x8004;
        /// <summary>Used in SetupDiClassDevs to get devices present in the system</summary>
        public const int DIGCF_PRESENT = 0x02;
        /// <summary>Used in SetupDiClassDevs to get device interface details</summary>
        public const int DIGCF_DEVICEINTERFACE = 0x10;
        /// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
        public const int DEVTYP_DEVICEINTERFACE = 0x05;
        /// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        /// <summary>Purges Win32 transmit buffer by aborting the current transmission.</summary>
        public const int PURGE_TXABORT = 0x01;
        /// <summary>Purges Win32 receive buffer by aborting the current receive.</summary>
        public const int PURGE_RXABORT = 0x02;
        /// <summary>Purges Win32 transmit buffer by clearing it.</summary>
        public const int PURGE_TXCLEAR = 0x04;
        /// <summary>Purges Win32 receive buffer by clearing it.</summary>
        public const int PURGE_RXCLEAR = 0x08;
        /// <summary>CreateFile : Open file for read</summary>
        public const int GENERIC_READ = unchecked((int) 0x80000000);
        /// <summary>CreateFile : Open file for write</summary>
        public const int GENERIC_WRITE = 0x40000000;
        /// <summary>CreateFile : file share for write</summary>
        public const int FILE_SHARE_WRITE = 0x2;
        /// <summary>CreateFile : file share for read</summary>
        public const int FILE_SHARE_READ = 0x1;
        /// <summary>CreateFile : Open handle for overlapped operations</summary>
        public const int FILE_FLAG_OVERLAPPED = 0x40000000;
        /// <summary>CreateFile : Resource to be "created" must exist</summary>
        public const int OPEN_EXISTING = 3;
        /// <summary>CreateFile : Resource will be "created" or existing will be used</summary>
        public const int OPEN_ALWAYS = 4;
        /// <summary>ReadFile/WriteFile : Overlapped operation is incomplete.</summary>
        public const int ERROR_IO_PENDING = 997;
        /// <summary>Infinite timeout</summary>
        public const int INFINITE = unchecked((int) 0xFFFFFFFF);
        /// <summary>Simple representation of a null handle : a closed stream will get this handle. Note it is public for comparison by higher level classes.</summary>
        public static IntPtr NullHandle = IntPtr.Zero;
        /// <summary>Simple representation of the handle returned when CreateFile fails.</summary>
        public static IntPtr InvalidHandleValue = new IntPtr(-1);
        #endregion

        #region P/Invoke
        /// <summary>
        /// Gets the GUID that Windows uses to represent HID class devices
        /// </summary>
        /// <param name="gHid">An out parameter to take the Guid</param>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern void HidD_GetHidGuid(out Guid gHid);

        /// <summary>
        /// Allocates an InfoSet memory block within Windows that contains details of devices.
        /// </summary>
        /// <param name="gClass">Class guid (e.g. HID guid)</param>
        /// <param name="strEnumerator">Not used</param>
        /// <param name="hParent">Not used</param>
        /// <param name="nFlags">Type of device details required (DIGCF_ constants)</param>
        /// <returns>A reference to the InfoSet</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, [MarshalAs(UnmanagedType.LPStr)] string strEnumerator, IntPtr hParent, int nFlags);

        /// <summary>
        /// Frees InfoSet allocated in call to above.
        /// </summary>
        /// <param name="lpInfoSet">Reference to InfoSet</param>
        /// <returns>true if successful</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern int SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

        /// <summary>
        /// Gets the DeviceInterfaceData for a device from an InfoSet.
        /// </summary>
        /// <param name="lpDeviceInfoSet">InfoSet to access</param>
        /// <param name="nDeviceInfoData">Not used</param>
        /// <param name="gClass">Device class guid</param>
        /// <param name="nIndex">Index into InfoSet for device</param>
        /// <param name="oInterfaceData">DeviceInterfaceData to fill with data</param>
        /// <returns>True if successful, false if not (e.g. when index is passed end of InfoSet)</returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr lpDeviceInfoSet, 
            int nDeviceInfoData, 
            ref Guid gClass, 
            int nIndex, 
            ref DeviceInterfaceData oInterfaceData);

        /// <summary>
        /// SetupDiGetDeviceInterfaceDetail
        /// Gets the interface detail from a DeviceInterfaceData. This is pretty much the device path.
        /// You call this twice, once to get the size of the struct you need to send (nDeviceInterfaceDetailDataSize=0)
        /// and once again when you've allocated the required space.
        /// </summary>
        /// <param name="lpDeviceInfoSet">InfoSet to access</param>
        /// <param name="oInterfaceData">DeviceInterfaceData to use</param>
        /// <param name="lpDeviceInterfaceDetailData">DeviceInterfaceDetailData to fill with data</param>
        /// <param name="nDeviceInterfaceDetailDataSize">The size of the above</param>
        /// <param name="nRequiredSize">The required size of the above when above is set as zero</param>
        /// <param name="lpDeviceInfoData">Not used</param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr lpDeviceInfoSet, 
            ref DeviceInterfaceData oInterfaceData, 
            IntPtr lpDeviceInterfaceDetailData, 
            int nDeviceInterfaceDetailDataSize, 
            ref int nRequiredSize, 
            IntPtr lpDeviceInfoData);

        /// <summary>
        /// SetupDiGetDeviceInterfaceDetail
        /// Gets the interface detail from a DeviceInterfaceData. This is pretty much the device path.
        /// You call this twice, once to get the size of the struct you need to send (nDeviceInterfaceDetailDataSize=0)
        /// and once again when you've allocated the required space.
        /// </summary>
        /// <param name="lpDeviceInfoSet">InfoSet to access</param>
        /// <param name="oInterfaceData">DeviceInterfaceData to use</param>
        /// <param name="oDetailData">DeviceInterfaceDetailData to fill with data</param>
        /// <param name="nDeviceInterfaceDetailDataSize">The size of the above</param>
        /// <param name="nRequiredSize">The required size of the above when above is set as zero</param>
        /// <param name="lpDeviceInfoData">Not used</param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr lpDeviceInfoSet, 
            ref DeviceInterfaceData oInterfaceData, 
            ref DeviceInterfaceDetailData oDetailData, 
            int nDeviceInterfaceDetailDataSize, 
            ref int nRequiredSize, 
            IntPtr lpDeviceInfoData);

        /// <summary>
        /// Registers a window for device insert/remove messages
        /// </summary>
        /// <param name="hwnd">Handle to the window that will receive the messages</param>
        /// <param name="oInterface">DeviceBroadcastInterrface structure</param>
        /// <param name="nFlags">set to DEVICE_NOTIFY_WINDOW_HANDLE</param>
        /// <returns>A handle used when unregistering</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hwnd, DeviceBroadcastInterface oInterface, int nFlags);

        /// <summary>
        /// Unregister from above.
        /// </summary>
        /// <param name="hHandle">Handle returned in call to RegisterDeviceNotification</param>
        /// <returns>True if success</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterDeviceNotification(IntPtr hHandle);

        /// <summary>
        /// Gets details from an open device. Reserves a block of memory which must be freed.
        /// </summary>
        /// <param name="hFile">Device file handle</param>
        /// <param name="lpData">Reference to the preparsed data block</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetPreparsedData(SafeFileHandle hFile, out IntPtr lpData);

        /// <summary>
        /// Frees the memory block reserved above.
        /// </summary>
        /// <param name="pData">Reference to preparsed data returned in call to GetPreparsedData</param>
        /// <returns></returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_FreePreparsedData(ref IntPtr pData);

        /// <summary>
        /// Gets a device's capabilities from the preparsed data.
        /// </summary>
        /// <param name="lpData">Preparsed data reference</param>
        /// <param name="oCaps">HidCaps structure to receive the capabilities</param>
        /// <returns>True if successful</returns>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern int HidP_GetCaps(IntPtr lpData, out HidPCaps oCaps);

        /// <summary>
        /// Get a device's attributes
        /// </summary>
        [DllImport("hid.dll", SetLastError = true)]
        public static extern bool HidD_GetAttributes(SafeFileHandle handle, ref HidDAttributes Attributes);
 
        /// <summary>
        /// Creates/opens a file, serial port, USB device... etc
        /// </summary>
        /// <param name="strName">Path to object to open</param>
        /// <param name="nAccess">Access mode. e.g. Read, write</param>
        /// <param name="nShareMode">Sharing mode</param>
        /// <param name="lpSecurity">Security details (can be null)</param>
        /// <param name="nCreationFlags">Specifies if the file is created or opened</param>
        /// <param name="nAttributes">Any extra attributes? e.g. open overlapped</param>
        /// <param name="lpTemplate">Not used</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            [MarshalAs(UnmanagedType.LPStr)] string strName, 
            int nAccess, int nShareMode, 
            IntPtr lpSecurity, 
            int nCreationFlags, 
            int nAttributes, 
            IntPtr lpTemplate);

        /// <summary>
        /// Closes a window handle. File handles, event handles, mutex handles... etc
        /// </summary>
        /// <param name="hFile">Handle to close</param>
        /// <returns>True if successful.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int CloseHandle(SafeFileHandle hFile);

        #endregion
    }
}
