﻿using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Groupon.WizardPages
{
    /// <summary>
    /// Wizard page for configuring Volusion access
    /// </summary>
    public partial class GrouponStoreAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponStoreAccountPage()
        {
            InitializeComponent();

            helpLink.Url = GrouponStoreType.AccountSettingsHelpUrl;
        }

        /// <summary>
        /// User is moving to the next wizard page, perform any autoconfiguration or credentials saving
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            GrouponStoreEntity store = GetStore<GrouponStoreEntity>();

            store.SupplierID = supplierIDTextbox.Text;
            store.Token = tokenTextBox.Text;

            if (supplierIDTextbox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Supplier ID");
                e.NextPage = this;
                return;
            }

            if (tokenTextBox.Text.Length == 0)
            {
                MessageHelper.ShowError(this, "Please enter your Token");
                e.NextPage = this;
                return;
            }

            try
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var client = lifetimeScope.Resolve<IGrouponWebClient>();

                    //Check to see if we have access to Groupon with the new creds
                    //Ask for some orders
                    client.GetOrders(store, DateTime.UtcNow, 1);
                }
            }
            catch (GrouponException ex)
            {
                ShowConnectionException(ex);
                e.NextPage = this;
                return;
            }

            GrouponTemplate.InstallGrouponTemplate();
        }

        /// <summary>
        /// Hook to allow derivatives add custom error handling for connectivity testing failures.
        /// Return true to indicate the error has been handled.
        /// </summary>
        protected virtual void ShowConnectionException(GrouponException ex)
        {
            MessageHelper.ShowError(this, ex.Message);
        }
    }
}
