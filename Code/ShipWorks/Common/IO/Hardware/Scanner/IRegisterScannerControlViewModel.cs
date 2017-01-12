using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    public interface IRegisterScannerControlViewModel
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
        /// Gets or sets the save scanner command.
        /// </summary>
        ICommand SaveScannerCommand { get; set; }

        /// <summary>
        /// Gets or sets the action to close the parent dialog.
        /// </summary>
        Action CloseDialog { get; set; }
    }
}