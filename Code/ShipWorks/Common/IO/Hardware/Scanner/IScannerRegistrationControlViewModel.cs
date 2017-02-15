using System;
using System.Windows.Input;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// View model handling logic for the RegisterScannerControl
    /// </summary>
    public interface IScannerRegistrationControlViewModel
    {
        /// <summary>
        /// Gets a value indicating whether a scan result has been found.
        /// </summary>
        bool ResultFound { get; }

        /// <summary>
        /// The scan result.
        /// </summary>
        string ScanResult { get; set; }

        /// <summary>
        /// Gets or sets the save scanner command.
        /// </summary>
        ICommand SaveScannerCommand { get; set; }

        /// <summary>
        /// Gets or sets the action to close the parent dialog.
        /// </summary>
        Action CloseDialog { get; set; }
    }
}