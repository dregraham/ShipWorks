using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.SingleScan.ScannerServicePipelines;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Main entry point for interacting with scanners
    /// </summary>
    public class ScannerService : IInitializeForCurrentUISession
    {
        private readonly IUser32Devices user32Devices;
        private readonly IMainForm mainForm;
        private readonly IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar;
        private readonly IScannerMessageFilterFactory scannerMessageFilterFactory;
        private readonly IUserSession userSession;
        private readonly IDisposable subscriptions;
        private IScannerMessageFilter scannerMessageFilter;

        /// <summary>
        /// Constructor used for mocking purposes
        /// </summary>
        protected ScannerService()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>This class has too many dependencies. We could move the registrar dependency to the
        /// filters themselves.</remarks>
        [NDependIgnoreTooManyParams]
        public ScannerService(IUser32Devices user32Devices,
            IMainForm mainForm, IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar,
            IScannerMessageFilterFactory scannerMessageFilterFactory, IUserSession userSession,
            IEnumerable<IScannerServicePipeline> pipelines)
        {
            this.userSession = userSession;
            this.scannerMessageFilterFactory = scannerMessageFilterFactory;
            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            this.user32Devices = user32Devices;
            this.mainForm = mainForm;

            subscriptions = new CompositeDisposable(pipelines.Select(x => x.Register(this)).ToArray());
        }

        /// <summary>
        /// Disable scanner handling
        /// </summary>
        public virtual void Disable()
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
        public virtual void Enable()
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
        public virtual bool IsSingleScanEnabled()
        {
            int singleScanSetting = userSession.Settings?.SingleScanSettings ?? (int) SingleScanSettings.Disabled;
            return singleScanSetting != (int) SingleScanSettings.Disabled;
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            subscriptions.Dispose();
            Disable();
        }
    }
}
