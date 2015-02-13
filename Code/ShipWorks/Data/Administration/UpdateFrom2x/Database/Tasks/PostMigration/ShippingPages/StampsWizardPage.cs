using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post-migration wizard page for allowing the user to setup Stamps.com.
    /// 
    /// This should only be presented if there are Stamps shipments in the migrated 
    /// database.  V2 didn't have a "Stamps Shipper".
    /// </summary>
    public partial class StampsWizardPage : WizardPage
    {
        bool anyStamps;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsWizardPage()
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
                anyStamps = ShipmentCollection.GetCount(SqlAdapter.Default, ShipmentFields.ShipmentType == (int) ShipmentTypeCode.Stamps) > 0;
            }

            // If there are no UPS accounts or shipments just skip
            if (!anyStamps)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Stamps))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the shipper.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            using (UspsSetupWizard wizard = new UspsSetupWizard(promotion, true))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Stamps);

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
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.Stamps);
        }
    }
}
