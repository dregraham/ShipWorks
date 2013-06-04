using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using ShipWorks.Templates.Printing;
using ShipWorks.Common.IO.Hardware.Printers;
using System.Reflection;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// User control for selecting a printer and paper source
    /// </summary>
    public partial class PrinterSelectionControl : UserControl
    {
        bool showPrinterCalibration = false;

        class PrinterInfo
        {
            [Obfuscation(Exclude = true)]
            public string Name { get; set; }

            [Obfuscation(Exclude = true)]
            public string DisplayText { get; set; }

            public bool IsValid { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterSelectionControl()
        {
            InitializeComponent();

            // Start out with no printers or sources
            printer.Enabled = false;
            calibratePrinter.Enabled = false;
            paperSource.Enabled = false;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UpdatePrinterCalibrationDisplay();
        }

        /// <summary>
        /// Load the available system printers into the control, selecting the given printer and paper source as the initial selection.
        /// </summary>
        public void LoadPrinters(string selectedPrinter, int selectedPaperSource, PrinterSelectionInvalidPrinterBehavior preserveBehavior)
        {
            List<string> installedPrinters;

            // Clear previous
            printer.DataSource = new List<PrinterInfo> { new PrinterInfo { Name = "", DisplayText = "", IsValid = false } };
            printer.DisplayMember = "DisplayText";
            printer.ValueMember = "Name";

            try
            {
                installedPrinters = PrintUtility.InstalledPrinters;

                errorProvider.SetError(printer, null);
                printer.Width = paperSource.Width;
            }
            catch (PrintingException ex)
            {
                installedPrinters = new List<string>();

                errorProvider.SetError(printer, ex.Message);
                printer.Width = paperSource.Width - 20;
            }

            // Create a list of objects that we will bind based on the printer names
            List<PrinterInfo> printerList = installedPrinters.Select(p => new PrinterInfo { Name = p, DisplayText = p, IsValid = true }).ToList();

            // If preservation is requested and they are trying to use an invalid printer then add it to the list so it can be selected and name preserved
            if (!string.IsNullOrWhiteSpace(selectedPrinter) && !installedPrinters.Contains(selectedPrinter) && ((preserveBehavior & PrinterSelectionInvalidPrinterBehavior.OnInvalidPreserve) != 0))
            {
                printerList.Insert(0, new PrinterInfo { Name = selectedPrinter, DisplayText = string.Format("{0} (Missing)", selectedPrinter), IsValid = false });
            }

            // If no printer was selected and preservation is required then add it to the list so the non-selection can be preserved
            if (string.IsNullOrWhiteSpace(selectedPrinter) && ((preserveBehavior & PrinterSelectionInvalidPrinterBehavior.OnNotChosenPreserve) != 0))
            {
                printerList.Insert(0, new PrinterInfo { Name = selectedPrinter, DisplayText = "(None Selected)", IsValid = false });
            }

            printer.Enabled = (printerList.Count > 0);
            calibratePrinter.Enabled = printer.Enabled;

            if (printerList.Count > 0)
            {
                // Bind the list
                printer.DataSource = printerList;

                // If the one they request is in the list default to it being selected
                if (printerList.Any(p => p.Name == selectedPrinter))
                {
                    printer.SelectedValue = selectedPrinter;
                }
                // Otherwise default to the system default
                else if (installedPrinters.Count > 0)
                {
                    printer.SelectedValue = new PrinterSettings().PrinterName;
                }
            }
            else
            {
                printer.DataSource = new List<PrinterInfo> { new PrinterInfo { Name = "", DisplayText = "<No Installed Printers>", IsValid = false } };
            }

            // Ensure a selection
            if (printer.SelectedIndex < 0)
            {
                printer.SelectedIndex = 0;
            }

            PrinterInfo selectedInfo = (PrinterInfo) printer.SelectedItem;

            // If there is now a valid printer selected, also select the paper source
            if (selectedInfo.IsValid)
            {
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.PrinterName = selectedInfo.Name;

                // If the requested selection wasn't chosen or isn't valid then the selected paper source doesn't matter
                if (selectedInfo.Name != selectedPrinter || !selectedInfo.IsValid)
                {
                    selectedPaperSource = (int) PaperSourceKind.AutomaticFeed;
                }

                // Set the value
                paperSource.SelectedValue = selectedPaperSource;

                // Set the default source
                if (paperSource.SelectedIndex == -1 && printerSettings.DefaultPageSettings != null)
                {
                    paperSource.SelectedValue = printerSettings.DefaultPageSettings.PaperSource.RawKind;
                }

                // Fallback to the first one if necessary
                if (paperSource.SelectedIndex == -1 && paperSource.Items.Count > 0)
                {
                    paperSource.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Controls if the printer calibration button is shown.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowPrinterCalibration
        {
            get
            {
                return showPrinterCalibration;
            }
            set
            {
                if (showPrinterCalibration != value)
                {
                    showPrinterCalibration = value;

                    UpdatePrinterCalibrationDisplay();
                }
            }
        }

        /// <summary>
        /// The selected printer
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PrinterName
        {
            get
            {
                return ((PrinterInfo) printer.SelectedItem).Name;
            }
        }

        /// <summary>
        /// The paper tray to use on the selected printer
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PaperSource
        {
            get
            {
                if (paperSource.Enabled && paperSource.Items.Count > 0)
                {
                    return (int) paperSource.SelectedValue;
                }

                return (int) PaperSourceKind.AutomaticFeed;
            }
        }

        /// <summary>
        /// Changing selected printer
        /// </summary>
        private void OnChangePrinter(object sender, System.EventArgs e)
        {
            LoadPrinterPaperSources();

            calibratePrinter.Enabled = ((PrinterInfo) printer.SelectedItem).IsValid;
        }

        /// <summary>
        /// Load all the paper sources for the currently selected printer
        /// </summary>
        private void LoadPrinterPaperSources()
        {
            Cursor.Current = Cursors.WaitCursor;

            // Get paper source combo ready
            paperSource.DataSource = null;
            paperSource.DisplayMember = "SourceName";
            paperSource.ValueMember = "RawKind";

            PrinterInfo printerInfo = (PrinterInfo) printer.SelectedItem;

            // Get out if no valid printer is selected
            if (!printerInfo.IsValid)
            {
                paperSource.Enabled = false;
                return;
            }

            // Create settings for selected printer
            PrinterSettings settings = new PrinterSettings();
            settings.PrinterName = printerInfo.Name;

            // Add each PaperSource
            paperSource.DataSource = settings.PaperSources.Cast<PaperSource>().ToList();

            // If nothing is selected, select the default
            if (paperSource.SelectedIndex == -1 && paperSource.Items.Count > 0)
            {
                paperSource.SelectedValue = settings.DefaultPageSettings.PaperSource.RawKind;

                // Ensure a selection
                if (paperSource.SelectedIndex == -1)
                {
                    paperSource.SelectedIndex = 0;
                }
            }

            // If no paper sources were found, disable the box
            paperSource.Enabled = (paperSource.Items.Count > 0);
        }

        /// <summary>
        /// Updates the visibility of the printer calibration section
        /// </summary>
        private void UpdatePrinterCalibrationDisplay()
        {
            labelCalibrate.Visible = showPrinterCalibration;
            calibratePrinter.Visible = showPrinterCalibration;

            if (showPrinterCalibration)
            {
                Height = calibratePrinter.Bottom + 8;
            }
            else
            {
                Height = paperSource.Bottom + 8;
            }
        }

        /// <summary>
        /// Calibrate the selected printer and source
        /// </summary>
        private void OnCalibratePrinter(object sender, EventArgs e)
        {
            using (PrinterCalibrationWizard dlg = new PrinterCalibrationWizard(PrinterName, PaperSource))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
