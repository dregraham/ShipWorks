﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore;
using ShipWorks.Users;

namespace ShipWorks.Common.IO.Hardware.Scanner
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

        private IScannerMessageFilter scannerMessageFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerService(IScannerIdentifier scannerIdentifier, IUser32Devices user32Devices,
            Func<IWin32Window> getWindow, IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar,
            IScannerMessageFilterFactory scannerMessageFilterFactory, IUserSession userSession)
        {
            this.scannerMessageFilterFactory = scannerMessageFilterFactory;
            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            this.getWindow = getWindow;
            this.user32Devices = user32Devices;
            this.scannerIdentifier = scannerIdentifier;

            //(SingleScanSettings) userSession.User.Settings.SingleScanSettings
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            //throw new NotImplementedException();
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
