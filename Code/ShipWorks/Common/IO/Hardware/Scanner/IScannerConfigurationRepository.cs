using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Save and load scanner configuration
    /// </summary>
    [Service]
    public interface IScannerConfigurationRepository
    {
        /// <summary>
        /// Save the name of the scanner
        /// </summary>
        void SaveScannerName(string name);

        /// <summary>
        /// Clears out the scanner name from scanner.xml
        /// </summary>
        void ClearScannerName();

        /// <summary>
        /// Get the name of the current scanner
        /// </summary>
        string GetScannerName();
    }
}