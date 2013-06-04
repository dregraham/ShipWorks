using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.UI;
using System.Drawing.Printing;
using ShipWorks.UI.Controls.Html;
using ShipWorks.Templates.Printing;
using System.Threading;
using Interapptive.Shared;
using ShipWorks.Templates.Processing;
using ShipWorks.Common.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Wizard for calibrating a printer for label printing
    /// </summary>
    public partial class PrinterCalibrationWizard : WizardForm
    {
        string initialPrinter;
        int initialSource;

        // The current calibration
        PrinterCalibration calibration;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterCalibrationWizard(string printer, int paperSource)
        {
            InitializeComponent();

            this.initialPrinter = printer;
            this.initialSource = paperSource;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            printerControl.LoadPrinters(initialPrinter, initialSource, PrinterSelectionInvalidPrinterBehavior.NeverPreserve);
        }

        /// <summary>
        /// Stepping next from the page where the printer is selected
        /// </summary>
        private void OnPrinterSelectionStepNext(object sender, WizardStepEventArgs e)
        {
            if (printerControl.PrinterName.Length == 0)
            {
                MessageHelper.ShowMessage(this, "A printer must be selected.");
                e.NextPage = CurrentPage;
                return;
            }

            calibration = PrinterCalibration.Load(printerControl.PrinterName, printerControl.PaperSource);

            xOffset.Text = calibration.XOffset.ToString();
            yOffset.Text = calibration.YOffset.ToString();
        }

        /// <summary>
        /// Print the calibration page
        /// </summary>
        private void OnPrint(object sender, EventArgs e)
        {
            ProgressProvider progressProvider = new ProgressProvider();

            ProgressItem progressItem = new ProgressItem("Printing");
            progressItem.CanCancel = false;
            progressItem.Detail = "Sending page to printer...";
            progressItem.Starting();
            progressProvider.ProgressItems.Add(progressItem);

            // Show the progress window
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Printing";
            progressDlg.Description = "ShipWorks is printing the calibration page.";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            double pageWidth = paperSizeControl.PaperWidth;
            double pageHeight = paperSizeControl.PaperHeight;

            // Our A and B directions for entering the offset values assume non landscape.  So we force that.
            if (paperSizeControl.PaperWidth > paperSizeControl.PaperHeight)
            {
                pageWidth = paperSizeControl.PaperHeight;
                pageHeight = paperSizeControl.PaperWidth;
            }

            PrinterCalibrationPagePrinter printer = new PrinterCalibrationPagePrinter();
            printer.PrintCompleted += new AsyncCompletedEventHandler(OnPrintCompleted);
            printer.PrintAsync(printerControl.PrinterName, printerControl.PaperSource, pageWidth, pageHeight, delayer);

            delayer.ShowAfter(this, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Printing has completed
        /// </summary>
        void OnPrintCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((AsyncCompletedEventHandler) OnPrintCompleted, sender, e);
                return;
            }

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) e.UserState;
            delayer.NotifyComplete();

            PrinterCalibrationPagePrinter printer = (PrinterCalibrationPagePrinter) sender;
            printer.PrintCompleted -= this.OnPrintCompleted;

            // Check for errors
            if (e.Error != null)
            {
                // See if its an error we know how to handle.
                PrintingException printingEx = e.Error as PrintingException;
                if (printingEx != null)
                {
                    PrintingExceptionDisplay.ShowError(this, printingEx);
                }

                // Rethrow the error as is
                else
                {
                    throw new ApplicationException(e.Error.Message, e.Error);
                }
            }
        }

        /// <summary>
        /// Stepping next on the finish page - save the calibration results
        /// </summary>
        private void OnFinishStepNext(object sender, WizardStepEventArgs e)
        {
            decimal xOffsetValue;
            if (decimal.TryParse(xOffset.Text, out xOffsetValue))
            {
                calibration.XOffset = xOffsetValue;
            }

            decimal yOffsetValue;
            if (decimal.TryParse(yOffset.Text, out yOffsetValue))
            {
                calibration.YOffset = yOffsetValue;
            }

            calibration.Save();
        }
    }
}
