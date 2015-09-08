using System;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Data.Adapter.Custom;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post migration page for configuring an Other shipper
    /// </summary>
    public partial class OtherShipperWizardPage : WizardPage
    {
        static bool anyOtherShipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public OtherShipperWizardPage()
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
                anyOtherShipments = OtherShipmentCollection.GetCount(SqlAdapter.Default) > 0;
            }

            // If there are no Other shipments, then just skip
            if (!anyOtherShipments)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Other))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the 'Other' shipment type
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ShipmentTypeSetupWizardForm wizard = lifetimeScope.ResolveKeyed<ShipmentTypeSetupWizardForm>(ShipmentTypeCode.Other))
                {
                    if (wizard.ShowDialog(this) == DialogResult.OK)
                    {
                        // record that Other is now configured
                        ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Other);

                        ShowConfiguredUI();
                    }
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
            // We'll only be here if we found Other shipments, in which case we have to at least mark the type as activated
            // so they can be seen in the shipment window.
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.Other);
        }
    }
}
