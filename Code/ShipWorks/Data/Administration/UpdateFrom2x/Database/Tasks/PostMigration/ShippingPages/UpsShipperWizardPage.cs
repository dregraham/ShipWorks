using System;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post migration page for configuring an UPS shipper
    /// </summary>
    public partial class UpsShipperWizardPage : WizardPage
    {
        static bool anyUpsOlt;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipperWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page, determine if it should even be shown.
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.FirstTime)
            {
                anyUpsOlt = UpsAccountCollection.GetCount(SqlAdapter.Default) > 0;

                if (!anyUpsOlt)
                {
                    anyUpsOlt = ShipmentCollection.GetCount(SqlAdapter.Default, ShipmentFields.ShipmentType == (int) ShipmentTypeCode.UpsOnLineTools) > 0;
                }
            }

            // If there are no UPS accounts or shipments just skip
            if (!anyUpsOlt)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.UpsOnLineTools))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the shipper.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            using (UpsSetupWizard wizard = new UpsSetupWizard(ShipmentTypeCode.UpsOnLineTools))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    // record that UpsOnLineTools is now configured
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);

                    ShowConfiguredUI();
                }
            }
        }

        /// <summary>
        /// Show the UI that shows it's already configured
        /// </summary>
        private void ShowConfiguredUI()
        {
            linkConfigure.Visible = false;
            panelConfigured.Visible = true;
            panelConfigured.Top = linkConfigure.Top;
        }

        /// <summary>
        /// Moving next from the wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // We'll only be here if we found UpsOnLineTools shipments, in which case we have to at least mark the type as activated
            // so they can be seen in the shipment window.
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.UpsOnLineTools);
        }
    }
}
