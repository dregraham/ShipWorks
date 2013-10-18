using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using ShipWorks.Shipping;

namespace ShipWorks.Templates.Printing.Configuration
{
    /// <summary>
    /// Control for selecting the primary printers that will be used by shipworks
    /// </summary>
    public partial class PrinterFormatControl : UserControl
    {
        const string notSelected = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterFormatControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            printer.LoadPrinters(notSelected, -1, Media.PrinterSelectionInvalidPrinterBehavior.OnNotChosenPreserve);
        }

        /// <summary>
        /// The selected printer has changed
        /// </summary>
        private void OnPrinterChanged(object sender, EventArgs e)
        {
            panelPaperType.Visible = printer.PrinterName != notSelected;
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
        /// The selected paper type has changed
        /// </summary>
        private void OnPaperTypeChanged(object sender, EventArgs e)
        {
            panelThermal.Enabled = radioThermal.Checked;
        }

        /// <summary>
        /// The type of thermal labels is changing
        /// </summary>
        private void OnChangeThermalType(object sender, EventArgs e)
        {
            if (thermalLanguage.SelectedIndex == 0)
            {
                using (PrinterThermalLanguageWizard dlg = new PrinterThermalLanguageWizard(printer.PrinterName))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        if (dlg.ThermalLanguage == ThermalLabelType.EPL)
                        {
                            thermalLanguage.SelectedIndex = 2;
                        }
                        else if (dlg.ThermalLanguage == ThermalLabelType.ZPL)
                        {
                            thermalLanguage.SelectedIndex = 1;
                        }
                        else
                        {
                            thermalLanguage.SelectedIndex = 3;
                        }
                    }
                    else
                    {
                        thermalLanguage.SelectedIndex = -1;
                    }
                }
            }
        }
    }
}
