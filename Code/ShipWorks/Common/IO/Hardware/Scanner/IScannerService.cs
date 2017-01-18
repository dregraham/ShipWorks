using ShipWorks.ApplicationCore.ComponentRegistration;

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
        /// Based on SingleScan settings, return true if single scan is enabled
        /// </summary>
        bool IsSingleScanEnabled();
    }
}
