using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.FactoryClasses;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post-migration page for configuring existing Endicia shippers, or a new Endicia account if 
    /// no EndiciaShippers are defined but there exist at least one Endicia shipment from 2x.
    /// </summary>
    public partial class EndiciaWizardPage : WizardPage
    {
        bool anyEndicia;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the wizard page for the first time
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.FirstTime)
            {
                anyEndicia = EndiciaAccountCollection.Fetch(SqlAdapter.Default, EndiciaAccountFields.EndiciaReseller == (int)EndiciaReseller.None).Any();

                if (!anyEndicia)
                {
                    RelationPredicateBucket bucket = new RelationPredicateBucket(ShipmentFields.ShipmentType == (int)ShipmentTypeCode.Endicia);
                    anyEndicia = SqlAdapter.Default.GetDbCount(new ShipmentEntityFactory().CreateFields(), bucket) > 0;
                    //anyEndicia = EndiciaShipmentCollection.GetCount(SqlAdapter.Default, ShipmentFields.ShipmentType == (int)ShipmentTypeCode.Endicia) > 0;
                }
            }

            // If there are no endicia accounts or shipments just skip
            if (!anyEndicia)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Endicia))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the shipper.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            // We could have gotten through the "Signup" phase - which means we're not pending anymore, but still have not been fully Configured
            // if they canceled before getting there endicia account# emailed to them.
            using (EndiciaSetupWizard wizard = new EndiciaSetupWizard())
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Endicia);

                    Wizard.MoveNext();
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
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.Endicia);
        }

    }
}
