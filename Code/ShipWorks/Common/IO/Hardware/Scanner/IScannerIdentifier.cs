﻿using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Manage identification of the current scanner
    /// </summary>
    [Service]
    public interface IScannerIdentifier
    {
        /// <summary>
        /// Gets the current scanner state
        /// </summary>
        ScannerState ScannerState { get; }

        /// <summary>
        /// Save the specified handle as the current scanner
        /// </summary>
        void Save(IntPtr deviceHandle);

        /// <summary>
        /// Is the specified handle the current scanner?
        /// </summary>
        bool IsScanner(IntPtr deviceHandle);

        /// <summary>
        /// Handle a device being added to Windows
        /// </summary>
        void HandleDeviceAdded(IntPtr deviceHandle);

        /// <summary>
        /// Handle a device being removed from Windows
        /// </summary>
        void HandleDeviceRemoved(IntPtr deviceHandle);
    }
}
