using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using Interapptive.Shared;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Utility class for displaying printing error exceptions
    /// </summary>
    static class PrintingExceptionDisplay
    {
        /// <summary>
        /// Display the error message for the given PrintingException using the specified window owner.
        /// </summary>
        public static void ShowError(IWin32Window owner, PrintingException error)
        {
            PrinterConfigurationSecurityException configEx = error as PrinterConfigurationSecurityException;
            if (configEx != null)
            {
                using (PrinterConfigurationErrorDlg dlg = new PrinterConfigurationErrorDlg(configEx.Printer))
                {
                    dlg.ShowDialog(owner);
                }

                return;
            }

            PrintingNoTemplateOutputException noOutputEx = error as PrintingNoTemplateOutputException;
            if (noOutputEx != null)
            {
                MessageHelper.ShowWarning(owner, noOutputEx.Message);

                return;
            }

            MessageHelper.ShowError(owner, error.Message);
        }
    }
}
