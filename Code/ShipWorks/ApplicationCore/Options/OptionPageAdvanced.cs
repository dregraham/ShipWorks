using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Users;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Logon;
using ShipWorks.Shipping.ShipSense.Settings;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Advanced page of the Options window
    /// </summary>
    public partial class OptionPageAdvanced : OptionPageBase
    {
        ConfigurationEntity config;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageAdvanced()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Do initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ConfigurationData.CheckForChangesNeeded();
            config = ConfigurationData.Fetch();

            EnumHelper.BindComboBox<LogonMethod>(logOnMethod);
            logOnMethod.SelectedValue = (LogonMethod) config.LogOnMethod;

            addressCasing.Checked = config.AddressCasing;

            compareCustomerEmail.Checked = config.CustomerCompareEmail;
            compareCustomerAddress.Checked = config.CustomerCompareAddress;

            updateCustomerBilling.Checked = config.CustomerUpdateBilling;
            updateCustomerShipping.Checked = config.CustomerUpdateShipping;

            auditNewOrders.Checked = config.AuditNewOrders;
            auditDeletedOrders.Checked = config.AuditDeletedOrders;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            enableShipSense.Checked = settings.ShipSenseEnabled;
            resetKnowledgebase.Visible = Users.UserSession.User.IsAdmin;
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void Save()
        {
            config.LogOnMethod = (int) logOnMethod.SelectedValue;
            config.AddressCasing = addressCasing.Checked;

            config.CustomerCompareEmail = compareCustomerEmail.Checked;
            config.CustomerCompareAddress = compareCustomerAddress.Checked;

            config.CustomerUpdateBilling = updateCustomerBilling.Checked;
            config.CustomerUpdateShipping = updateCustomerShipping.Checked;

            config.AuditNewOrders = auditNewOrders.Checked;
            config.AuditDeletedOrders = auditDeletedOrders.Checked;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.ShipSenseEnabled = enableShipSense.Checked;
            ShippingSettings.Save(settings);

            ConfigurationData.Save(config);
        }

        /// <summary>
        /// Called when the button to reset the knowledge base is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnResetKnowledgebase(object sender, EventArgs e)
        {
            const string ConfirmationText = @"Resetting the knowledge base will delete all of the information that ShipWorks " +
                "uses to create shipments based on your shipment history.\n\n" +
                "Do you wish to continue?";

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Question, MessageBoxButtons.YesNo, ConfirmationText);

            if (result == DialogResult.Yes)
            {
                new Knowledgebase().Reset(UserSession.User);
            }
        }

        /// <summary>
        /// Called when the button to edit ShipSense settings is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnEditShipSenseClick(object sender, EventArgs e)
        {
            using (ShipSenseUniquenessSettingsDlg dialog = new ShipSenseUniquenessSettingsDlg())
            {
                dialog.ShowDialog(this);
            }
        }
    }
}
