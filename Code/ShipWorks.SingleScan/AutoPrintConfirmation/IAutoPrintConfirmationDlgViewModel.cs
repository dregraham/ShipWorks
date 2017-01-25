using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// ViewModel for the AutoPrintConfirmationDlg
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IAutoPrintConfirmationDlgViewModel : IDisposable
    {
        /// <summary>
        /// Initializes the ViewModel with text to display and the barcode that when scan, accepts the dialog.
        /// </summary>
        void Load(string barcodeAcceptanceText, string title, string displayText);

        /// <summary>
        /// Sets Dialog Result to true and closes the dialog
        /// </summary>
        void Accept();

        /// <summary>
        /// Sets DialogResult to false and closes the dialog
        /// </summary>
        void Cancel();

        /// <summary>
        /// Property changed handler
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
    }
}
