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
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Logon;

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

            ConfigurationData.Save(config);
        }
    }
}
