using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Properties;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Media;
using ShipWorks.UI;
using Interapptive.Shared.UI;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Window for configuring the settings of a PrintJob
    /// </summary>
    public partial class PrintJobSettingsDlg : Form
    {
        PrintJobSettings settings;

        bool ignoreNumericLabelPositionChanges = false;
        bool ignoreChooserLabelPositionChanges = false;

        bool labelSettingsChanged = false;

        LabelSheetEntity originalSheet = null;

        decimal calibrationX = 0;
        decimal calibrationY = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintJobSettingsDlg(PrintJobSettings settings)
        {
            InitializeComponent();

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
        }

        /// <summary>
        /// The text to display on the button that confirms the user's choices.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AcceptButtonText
        {
            get { return ok.Text; }
            set { ok.Text = value; }
        }

        /// <summary>
        /// Indicates if the label sheet has changed in any way.  Only valid of the dialog result is OK and the template type is Label.
        /// </summary>
        [Browsable(false)]
        public bool LabelSettingsChanged
        {
            get { return labelSettingsChanged; }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Printer
            printerControl.LoadPrinters(settings.PrinterName, settings.PaperSource, PrinterSelectionInvalidPrinterBehavior.OnInvalidPreserve);

            // Copies
            copiesControl.Copies = settings.Copies;
            copiesControl.Collate = settings.Collate;

            if (settings.PageSettings != null)
            {
                optionControl.OptionPages.Remove(optionPageLabels);

                // Hide the label chooser
                groupBoxFirstLabel.Visible = false;

                // Hide the calibration
                printerControl.ShowPrinterCalibration = false;

                int originalPrinterHeight = groupBoxPrinter.Height;

                // Update positioning
                groupBoxPrinter.Height = printerControl.Bottom + 3;
                groupBoxCopies.Top = groupBoxPrinter.Bottom + 6;
                Height -= (groupBoxFirstLabel.Height + (originalPrinterHeight - groupBoxPrinter.Height));

                if (!settings.IsThermal)
                {
                    LoadPageSettings(settings.PageSettings);
                }
                else
                {
                    optionControl.OptionPages.Remove(optionPagePaper);

                    // Thermals are always collated
                    copiesControl.AllowChangeCollate = false;
                    copiesControl.Collate = true;
                }
            }
            else
            {
                optionControl.OptionPages.Remove(optionPagePaper);
                LoadLabelSettings(settings.LabelSettings);
            }
        }

        /// <summary>
        /// Load the page settings
        /// </summary>
        private void LoadPageSettings(PrintJobPageSettings pageSettings)
        {
            pageSetupControl.PaperWidth = pageSettings.PageWidth;
            pageSetupControl.PaperHeight = pageSettings.PageHeight;

            pageSetupControl.MarginLeft = pageSettings.MarginLeft;
            pageSetupControl.MarginRight = pageSettings.MarginRight;
            pageSetupControl.MarginTop = pageSettings.MarginTop;
            pageSetupControl.MarginBottom = pageSettings.MarginBottom;
        }

        /// <summary>
        /// Save the settings from the UI to the given insance
        /// </summary>
        private void SavePageSettings(PrintJobPageSettings pageSettings)
        {
            pageSettings.PageWidth = pageSetupControl.PaperWidth;
            pageSettings.PageHeight = pageSetupControl.PaperHeight;

            pageSettings.MarginLeft = pageSetupControl.MarginLeft;
            pageSettings.MarginRight = pageSetupControl.MarginRight;
            pageSettings.MarginTop = pageSetupControl.MarginTop;
            pageSettings.MarginBottom = pageSetupControl.MarginBottom;
        }

        /// <summary>
        /// Load the label settings
        /// </summary>
        private void LoadLabelSettings(PrintJobLabelSettings labelSettings)
        {
            labelSheetControl.LabelSheetID = labelSettings.LabelSheetID;
            labelPositionControl.LabelPosition = labelSettings.LabelPosition;

            LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(labelSettings.LabelSheetID);
            if (labelSheet != null)
            {
                originalSheet = new LabelSheetEntity(labelSheet.Fields.Clone());
            }

            PrinterCalibration calibration = PrinterCalibration.Load(printerControl.PrinterName, printerControl.PaperSource);
            calibrationX = calibration.XOffset;
            calibrationY = calibration.YOffset;
        }

        /// <summary>
        /// Save the settings from the uii to the given instance
        /// </summary>
        private bool SaveLabelSettings(PrintJobLabelSettings labelSettings)
        {
            LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(labelSheetControl.LabelSheetID);
            if (labelSheet == null)
            {
                return false;
            }

            if (originalSheet == null)
            {
                labelSettingsChanged = true;
            }
            else
            {
                if (originalSheet.PaperSizeHeight != labelSheet.PaperSizeHeight ||
                    originalSheet.PaperSizeWidth != labelSheet.PaperSizeWidth ||
                    originalSheet.Rows != labelSheet.Rows ||
                    originalSheet.Columns != labelSheet.Columns ||
                    originalSheet.LabelHeight != labelSheet.LabelHeight ||
                    originalSheet.LabelWidth != labelSheet.LabelWidth ||
                    originalSheet.MarginLeft != labelSheet.MarginLeft ||
                    originalSheet.MarginTop != labelSheet.MarginTop ||
                    originalSheet.VerticalSpacing != labelSheet.VerticalSpacing ||
                    originalSheet.HorizontalSpacing != labelSheet.HorizontalSpacing)
                {
                    labelSettingsChanged = true;
                }
            }

            if (labelPositionControl.LabelPosition.Row != labelSettings.LabelPosition.Row ||
                labelPositionControl.LabelPosition.Column != labelSettings.LabelPosition.Column)
            {
                labelSettingsChanged = true;
            }

            PrinterCalibration calibration = PrinterCalibration.Load(printerControl.PrinterName, printerControl.PaperSource);
            if (calibration.YOffset != calibrationY || calibration.XOffset != calibrationX)
            {
                labelSettingsChanged = true;
            }

            labelSettings.LabelSheetID = labelSheetControl.LabelSheetID;
            labelSettings.LabelPosition = labelPositionControl.LabelPosition;

            return true;
        }

        /// <summary>
        /// User changed the label sheet to use
        /// </summary>
        private void OnLabelSheetChanged(object sender, EventArgs e)
        {
            LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(labelSheetControl.LabelSheetID);

            // Update the sheet for the position chooser
            labelPositionControl.LabelSheet = labelSheet;

            // Update numerics
            firstLabelRow.Enabled = labelSheet != null;
            firstLabelColumn.Enabled = labelSheet != null;

            if (labelSheet != null)
            {
                firstLabelRow.Maximum = labelSheet.Rows;
                firstLabelColumn.Maximum = labelSheet.Columns;
            }
        }

        /// <summary>
        /// User has selected a new position from the chooser with the mouse
        /// </summary>
        private void OnLabelChooserPositionChanged(object sender, EventArgs e)
        {
            if (ignoreChooserLabelPositionChanges)
            {
                return;
            }

            ignoreNumericLabelPositionChanges = true;

            firstLabelRow.Value = labelPositionControl.LabelPosition.Row;
            firstLabelColumn.Value = labelPositionControl.LabelPosition.Column;

            ignoreNumericLabelPositionChanges = false;
        }

        /// <summary>
        /// User has directly entered a new label position to use
        /// </summary>
        private void OnLabelNumericPositionChanged(object sender, EventArgs e)
        {
            if (ignoreNumericLabelPositionChanges)
            {
                return;
            }

            ignoreChooserLabelPositionChanges = true;

            labelPositionControl.LabelPosition = new LabelPosition((int) firstLabelColumn.Value, (int) firstLabelRow.Value);

            ignoreChooserLabelPositionChanges = false;
        }

        /// <summary>
        /// User wants to save settings
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            // Printer
            settings.PrinterName = printerControl.PrinterName;
            settings.PaperSource = printerControl.PaperSource;

            // Copies
            settings.Copies = copiesControl.Copies;
            settings.Collate = copiesControl.Collate;

            // No other settings to save unless its thermal
            if (!settings.IsThermal)
            {
                if (settings.PageSettings != null)
                {
                    SavePageSettings(settings.PageSettings);
                }
                else
                {
                    if (!SaveLabelSettings(settings.LabelSettings))
                    {
                        MessageHelper.ShowError(this, "A label sheet has not been selected.");
                        return;
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
