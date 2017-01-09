using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Main entry point for interacting with scanners
    /// </summary>
    [Service]
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
    }
}
