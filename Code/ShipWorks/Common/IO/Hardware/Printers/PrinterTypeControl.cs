using System;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Control for selecting the primary printers that will be used by shipworks
    /// </summary>
    public partial class PrinterTypeControl : UserControl
    {
        string printerName;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterTypeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            EnumHelper.BindComboBox<ThermalLanguage>(thermalLanguage, null);
            thermalLanguage.SelectedIndex = -1;
        }

        /// <summary>
        /// Clicking the imagery of the paper types
        /// </summary>
        private void OnClickPaperTypeImage(object sender, EventArgs e)
        {
            radioPaper.Checked = (sender == picturePaper);
            radioThermal.Checked = (sender == pictureThermal);
        }

        /// <summary>
        /// The printer name that is selected, which will be used for the thermal determination wizard
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }

        /// <summary>
        /// Gets the selcted printer type. If the user hasn't made a selection, the message will be shown and null will be returned.
        /// </summary>
        public PrinterType GetPrinterType()
        {
            if (!radioPaper.Checked && !radioThermal.Checked)
            {
                MessageHelper.ShowInformation(this, "Please select what type of paper your printer uses.");
                return null;
            }

            if (radioThermal.Checked && thermalLanguage.SelectedIndex < 0)
            {
                MessageHelper.ShowInformation(this, "Please select the thermal language supported by your printer.");
                return null;
            }

            PrinterTechnology technology = radioThermal.Checked ? PrinterTechnology.Thermal : PrinterTechnology.Standard;
            ThermalLanguage language = ThermalLanguage.None;

            if (technology == PrinterTechnology.Thermal)
            {
                language = (ThermalLanguage) thermalLanguage.SelectedValue;
            }

            return new PrinterType(technology, language);
        }

        /// <summary>
        /// The selected paper type has changed
        /// </summary>
        private void OnPaperTypeChanged(object sender, EventArgs e)
        {
            panelThermal.Enabled = radioThermal.Checked;
        }

        /// <summary>
        /// User wants help choosing their thermal language
        /// </summary>
        private void OnHelpMeChooseThermalLanguage(object sender, EventArgs e)
        {
            using (ThermalPrinterLanguageWizard dlg = new ThermalPrinterLanguageWizard(printerName))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    thermalLanguage.SelectedValue = dlg.ThermalLanguage;
                }
            }
        }
    }
}
