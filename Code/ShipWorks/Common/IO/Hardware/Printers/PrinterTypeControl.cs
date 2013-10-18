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
                using (ThermalPrinterLanguageWizard dlg = new ThermalPrinterLanguageWizard(printerName))
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
