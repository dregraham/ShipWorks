﻿using System;
using System.Windows.Input;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// ViewModel for the AutoPrintConfirmationDlg
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [Service]
    public interface IAutoPrintConfirmationDlgViewModel : IDisposable
    {
        /// <summary>
        /// Initializes the ViewModel with text to display and the barcode that when scan, accepts the dialog.
        /// </summary>
        void Load(string barcodeAcceptanceText, string displayText);

        /// <summary>
        /// Gets or sets the method to close the window.
        /// If user cancels, we will pass in false, else pass in true
        /// </summary>
        Action<bool> Close { get; set; }

        /// <summary>
        /// Gets the continue click command.
        /// </summary>
        ICommand ContinueClickCommand { get; }

        /// <summary>
        /// Gets the cancel click command.
        /// </summary>
        ICommand CancelClickCommand { get; }

        /// <summary>
        /// Gets the text to display to the user.
        /// </summary>
        string DisplayText { get; }
    }
}
