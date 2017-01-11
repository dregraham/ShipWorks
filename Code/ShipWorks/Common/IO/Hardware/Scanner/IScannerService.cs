using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Options;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Main entry point for interacting with scanners
    /// </summary>
    [Service(SingleInstance = true, ExternallyOwned = true)]
    public interface IScannerService
    {
        /// <summary>
        /// Enable scanner handling
        /// </summary>
        void Enable();

        /// <summary>
        /// Disable scanner handling
        /// </summary>
        void Disable();

        /// <summary>
        /// Begin finding a current scanner
        /// </summary>
        void BeginFindScanner();

        /// <summary>
        /// End finding the current scanner
        /// </summary>
        void EndFindScanner();

        /// <summary>
        /// Get the state of the current scanner
        /// </summary>
        ScannerState CurrentScannerState { get; }

        /// <summary>
        /// Based on SingleScan settings, return true if single scan should be enabled
        /// </summary>
        /// <returns></returns>
        bool ShouldSingleScanBeEnabled();
    }
}
