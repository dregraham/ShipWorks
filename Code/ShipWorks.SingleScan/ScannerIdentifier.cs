using System;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Manage identification of the current scanner
    /// </summary>
    public class ScannerIdentifier : IScannerIdentifier
    {
        private readonly IUser32Devices deviceManager;
        private readonly IScannerConfigurationRepository configurationRepository;
        private IntPtr? scannerHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerIdentifier"/> class.
        /// </summary>
        /// <param name="deviceManager">The device manager.</param>
        /// <param name="configurationRepository">The configuration repository.</param>
        public ScannerIdentifier(IUser32Devices deviceManager, IScannerConfigurationRepository configurationRepository)
        {
            this.deviceManager = deviceManager;
            this.configurationRepository = configurationRepository;
        }

        /// <summary>
        /// Get the state of the current scanner
        /// </summary>
        public ScannerState ScannerState
        {
            get
            {
                if (configurationRepository.Get().IsNullOrWhiteSpace())
                {
                    return ScannerState.NotRegistered;
                }

                return scannerHandle.HasValue ? ScannerState.Attached : ScannerState.Detached;
            }
        }

        /// <summary>
        /// Handle a device being added to Windows
        /// </summary>
        public void HandleDeviceAdded(IntPtr deviceHandle)
        {
            // If we already know about a scanner or there is no registered scanner, we don't care 
            // that a device was added
            if (scannerHandle.HasValue || configurationRepository.Get().IsNullOrWhiteSpace())
            {
                return;
            }

            string deviceName = deviceManager.GetDeviceName(deviceHandle);
            if (!deviceName.IsNullOrWhiteSpace() && deviceName == configurationRepository.Get())
            {
                scannerHandle = deviceHandle;
            }
        }

        /// <summary>
        /// Handle a device being removed from Windows
        /// </summary>
        public void HandleDeviceRemoved(IntPtr deviceHandle)
        {
            if (scannerHandle == deviceHandle)
            {
                scannerHandle = null;
            }
        }

        /// <summary>
        /// Is the specified handle the current scanner?
        /// </summary>
        public bool IsScanner(IntPtr deviceHandle) => scannerHandle == deviceHandle;

        /// <summary>
        /// Save the specified handle as the current scanner
        /// </summary>
        public void Save(IntPtr deviceHandle)
        {
            string name = deviceManager.GetDeviceName(deviceHandle);
            configurationRepository.Save(name);
        }
    }
}
