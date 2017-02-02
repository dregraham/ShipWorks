using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.Settings.WizardPages
{
    /// <summary>
    /// Wizard page for setting up printing settings
    /// </summary>
    public partial class ShippingWizardPagePrinting : WizardPage
    {
        ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingWizardPagePrinting(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Stepping into the control
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {

        }

        /// <summary>
        /// Wizard page is visible
        /// </summary>
        private void OnPageShown(object sender, WizardPageShownEventArgs e)
        {
            if (e.FirstTime)
            {
                Cursor.Current = Cursors.WaitCursor;

                ShipmentPrintHelper.InstallDefaultRules(shipmentType.ShipmentTypeCode, true, this);

                printOutputControl.LoadSettings(shipmentType);
            }
        }

        /// <summary>
        /// Stepping next out of the control
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // Ensure each template selected for use has been configured with the printer to use
            if (!TemplatePrinterSelectionDlg.EnsureConfigured(this, new IPrintWithTemplates[] { printOutputControl }))
            {
                e.NextPage = this;
                return;
            }

            printOutputControl.SaveSettings();
        }
    }
}
