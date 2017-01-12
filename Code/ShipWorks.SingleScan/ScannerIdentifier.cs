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
        private readonly IUser32Devices user32Devices;
        private readonly IScannerConfigurationRepository configurationRepository;
        private IntPtr? scannerHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerIdentifier"/> class.
        /// </summary>
        public ScannerIdentifier(IUser32Devices user32Devices, IScannerConfigurationRepository configurationRepository)
        {
            this.user32Devices = user32Devices;
            this.configurationRepository = configurationRepository;
        }

        /// <summary>
        /// Handle a device being added to Windows
        /// </summary>
        public void HandleDeviceAdded(IntPtr deviceHandle)
        {
            // If we already know about a scanner or there is no registered scanner, we don't care
            // that a device was added
            if (scannerHandle.HasValue || configurationRepository.GetName().IsNullOrWhiteSpace())
            {
                return;
            }

            string deviceName = user32Devices.GetDeviceName(deviceHandle);
            if (!deviceName.IsNullOrWhiteSpace() && deviceName == configurationRepository.GetName())
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
        public bool IsRegisteredScanner(IntPtr deviceHandle) => scannerHandle == deviceHandle;

        /// <summary>
        /// Save the specified handle as the current scanner
        /// </summary>
        public void Save(IntPtr deviceHandle)
        {
            scannerHandle = deviceHandle;
            string name = user32Devices.GetDeviceName(deviceHandle);
            configurationRepository.Save(name);
        }
    }
}
