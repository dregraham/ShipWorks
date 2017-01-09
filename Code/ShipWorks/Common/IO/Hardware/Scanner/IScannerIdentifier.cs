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
        void Save(int deviceHandle);

        /// <summary>
        /// Is the specified handle the current scanner?
        /// </summary>
        bool IsScanner(int deviceHandle);

        /// <summary>
        /// Handle a device being added to Windows
        /// </summary>
        void HandleDeviceAdded(int deviceHandle);

        /// <summary>
        /// Handle a device being removed from Windows
        /// </summary>
        void HandleDeviceRemoved(int deviceHandle);
    }
}
