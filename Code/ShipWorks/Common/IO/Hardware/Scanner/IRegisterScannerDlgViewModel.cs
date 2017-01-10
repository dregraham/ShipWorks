using System.Windows.Input;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    public interface IRegisterScannerDlgViewModel
    {
        /// <summary>
        /// Message to displaying indicating we are waiting for a scan
        /// </summary>
        string WaitingMessage { get; }

        /// <summary>
        /// Gets a value indicating whether a scan result has been found.
        /// </summary>
        bool ResultFound { get; }

        /// <summary>
        /// The scan result.
        /// </summary>
        string ScanResult { get; set; }

        /// <summary>
        /// The window loaded command.
        /// </summary>
        ICommand WindowLoadedCommand { get; set; }

        /// <summary>
        /// Gets or sets the save scanner command.
        /// </summary>
        ICommand SaveScannerCommand { get; set; }
    }
}