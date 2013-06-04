using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Common.IO.Hardware.Printers;
using Interapptive.Shared.UI;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// Window for choosing just a printer and its paper source
    /// </summary>
    public partial class PrinterSelectionDlg : Form
    {
        string initialPrinter;
        int initialPaperSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterSelectionDlg(string printer, int paperSource)
        {
            InitializeComponent();

            this.initialPrinter = printer;
            this.initialPaperSource = paperSource;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            printerSelectionControl.LoadPrinters(initialPrinter, initialPaperSource, PrinterSelectionInvalidPrinterBehavior.AlwaysPreserve);
        }

        /// <summary>
        /// The name of the selected printer.  Only valid if DialogResult is OK
        /// </summary>
        public string PrinterName
        {
            get { return printerSelectionControl.PrinterName; }
        }

        /// <summary>
        /// The paper source to use for the selected printer.  Only valid if DialogResult is OK
        /// </summary>
        public int PaperSource
        {
            get { return printerSelectionControl.PaperSource; }
        }

        /// <summary>
        /// Save the given printer
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            try
            {
                if (!PrintUtility.InstalledPrinters.Contains(printerSelectionControl.PrinterName))
                {
                    MessageHelper.ShowError(this, "Please select a printer.");
                    return;
                }

                DialogResult = DialogResult.OK;
            }
            catch (PrintingException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                return;
            }
        }
    }
}
