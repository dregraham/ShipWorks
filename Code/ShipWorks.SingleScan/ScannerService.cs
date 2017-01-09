﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Main entry point for interacting with scanners
    /// </summary>
    public class ScannerService : IScannerService, IInitializeForCurrentUISession
    {
        private readonly IScannerIdentifier scannerIdentifier;
        private readonly IUser32Devices user32Devices;
        private readonly Func<IWin32Window> getWindow;
        private readonly IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar;
        private readonly IScannerMessageFilterFactory scannerMessageFilterFactory;
        private readonly IUserSession userSession;

        private IScannerMessageFilter scannerMessageFilter;
        private IScannerMessageFilter findScannerMessageFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerService(IScannerIdentifier scannerIdentifier, IUser32Devices user32Devices,
            Func<IWin32Window> getWindow, IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar,
            IScannerMessageFilterFactory scannerMessageFilterFactory, IUserSession userSession)
        {
            this.userSession = userSession;
            this.scannerMessageFilterFactory = scannerMessageFilterFactory;
            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            this.getWindow = getWindow;
            this.user32Devices = user32Devices;
            this.scannerIdentifier = scannerIdentifier;
        }

        /// <summary>
        /// Get the state of the current scanner
        /// </summary>
        public ScannerState CurrentScannerState => scannerIdentifier.ScannerState;

        /// <summary>
        /// Begin finding a current scanner
        /// </summary>
        public void BeginFindScanner()
        {
            findScannerMessageFilter = scannerMessageFilterFactory.CreateFindScannerMessageFilter();
            windowsMessageFilterRegistrar.AddMessageFilter(findScannerMessageFilter);
        }

        /// <summary>
        /// Disable scanner handling
        /// </summary>
        public void Disable()
        {
            if (scannerMessageFilter == null)
            {
                return;
            }

            windowsMessageFilterRegistrar.RemoveMessageFilter(scannerMessageFilter);
            scannerMessageFilter = null;

            user32Devices.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = 0x01,
                Usage = 0x06,
                Flags = (int) RawInputDeviceNotificationFlags.REMOVE,
                TargetHandle = (IntPtr) null,
            });
        }

        /// <summary>
        /// Enable scanner handling
        /// </summary>
        public void Enable()
        {
            if (scannerMessageFilter != null)
            {
                return;
            }

            scannerMessageFilter = scannerMessageFilterFactory.CreateMessageFilter();
            windowsMessageFilterRegistrar.AddMessageFilter(scannerMessageFilter);

            user32Devices.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = 0x01,
                Usage = 0x06,
                Flags = (int) (RawInputDeviceNotificationFlags.DEFAULT | RawInputDeviceNotificationFlags.DEVNOTIFY),
                TargetHandle = getWindow().Handle,
            });
        }

        /// <summary>
        /// End finding the current scanner
        /// </summary>
        public void EndFindScanner()
        {
           windowsMessageFilterRegistrar.RemoveMessageFilter(findScannerMessageFilter);
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession() => Disable();

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            int singleScanSetting = userSession.User?.Settings?.SingleScanSettings ?? (int) SingleScanSettings.Disabled;

            if (singleScanSetting != (int) SingleScanSettings.Disabled)
            {
// TODO: UI thread exception was happening here.  Commenting out until we get to this.
//                Enable();
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            Disable();
        }
    }
}
