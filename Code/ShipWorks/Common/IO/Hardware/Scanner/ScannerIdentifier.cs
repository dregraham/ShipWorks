﻿using System;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Manage identification of the current scanner
    /// </summary>
    public class ScannerIdentifier : IScannerIdentifier
    {
        /// <summary>
        /// Handle a device being added to Windows
        /// </summary>
        public void HandleDeviceAdded(int deviceHandle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handle a device being removed from Windows
        /// </summary>
        public void HandleDeviceRemoved(int deviceHandle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Is the specified handle the current scanner?
        /// </summary>
        public bool IsScanner(int deviceHandle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the specified handle as the current scanner
        /// </summary>
        public void Save(int deviceHandle)
        {
            throw new NotImplementedException();
        }
    }
}
