using System;
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
        private readonly IUser32Devices user32Devices;
        private readonly IMainForm mainForm;
        private readonly IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar;
        private readonly IScannerMessageFilterFactory scannerMessageFilterFactory;
        private readonly IUserSession userSession;
        private IScannerMessageFilter scannerMessageFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerService(IUser32Devices user32Devices,
            IMainForm mainForm, IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar,
            IScannerMessageFilterFactory scannerMessageFilterFactory, IUserSession userSession)
        {
            this.userSession = userSession;
            this.scannerMessageFilterFactory = scannerMessageFilterFactory;
            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            this.user32Devices = user32Devices;
            this.mainForm = mainForm;
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
                UsagePage = RawInputDeviceConstants.Keyboard.UsagePage,
                Usage = RawInputDeviceConstants.Keyboard.Usage,
                Flags = (int) RawInputDeviceNotificationFlags.RemoveDevice,
                TargetHandle = mainForm.Handle
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

            scannerMessageFilter = scannerMessageFilterFactory.CreateRegisteredScannerInputHandler();
            windowsMessageFilterRegistrar.AddMessageFilter(scannerMessageFilter);

            user32Devices.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = RawInputDeviceConstants.Keyboard.UsagePage,
                Usage = RawInputDeviceConstants.Keyboard.Usage,
                Flags = (int) (RawInputDeviceNotificationFlags.Default | RawInputDeviceNotificationFlags.DeviceNotify),
                TargetHandle = mainForm.Handle,
            });
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
            // We need to be on the UI thread for message pumps to work, so check
            // to see if an Invoke is required, and do so if it is.
            if (mainForm == null)
            {
                return;
            }

            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke((Action) InitializeForCurrentSession, null);
                return;
            }

            if (IsSingleScanEnabled())
            {
                Enable();
            }
        }

        /// <summary>
        /// Based on SingleScan settings, return true if single scan should be enabled
        /// </summary>
        public bool IsSingleScanEnabled()
        {
            int singleScanSetting = userSession.Settings?.SingleScanSettings ?? (int) SingleScanSettings.Disabled;
            return singleScanSetting != (int) SingleScanSettings.Disabled;
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
