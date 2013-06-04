using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Represents a single line of a FedEx account that needs migrated
    /// </summary>
    public partial class FedExAccountLineControl : UserControl
    {
        FedExAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExAccountLineControl(FedExAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
        }

        /// <summary>
        /// The account this control represents
        /// </summary>
        public FedExAccountEntity Account
        {
            get { return account; }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            labelAccount.Text = string.Format("Account#: {0}", account.AccountNumber);
            linkConfigure.Left = labelAccount.Right + 5;
            panelConfigured.Left = labelAccount.Right + 5;

            linkConfigure.Visible = string.IsNullOrEmpty(account.MeterNumber);
            panelConfigured.Visible = !string.IsNullOrEmpty(account.MeterNumber);
        }

        /// <summary>
        /// Configure the selected account for ShipWorks 3
        /// </summary>
        private void OnConfigure(object sender, EventArgs e)
        {
            using (FedExSetupWizard dlg = new FedExSetupWizard(account))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    linkConfigure.Visible = false;
                    panelConfigured.Visible = true;

                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.FedEx);
                }
            }
        }
    }
}
