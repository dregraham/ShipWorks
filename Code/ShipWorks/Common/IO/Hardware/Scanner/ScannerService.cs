using System;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Main entry point for interacting with scanners
    /// </summary>
    public class ScannerService : IScannerService, IInitializeForCurrentUISession
    {
        /// <summary>
        /// Get the state of the current scanner
        /// </summary>
        public ScannerState CurrentScannerState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enable scanner handling
        /// </summary>
        public void Enable()
        {
            throw new NotImplementedException();
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
        public void EndSession()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
