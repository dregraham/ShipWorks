using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Email;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.OrderMotion.WizardPages
{
    /// <summary>
    /// Wizard page for configuring the email account that ShipWorks will check for order emails
    /// </summary>
    public partial class OrderMotionEmailPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionEmailPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            OrderMotionStoreEntity store = GetStore<OrderMotionStoreEntity>();
            EmailAccountEntity account = store.OrderMotionEmailAccount;

            if (account == null)
            {
                panelNotSetup.Visible = true;
                emailAccountControl.Visible = false;
            }
            else
            {
                panelNotSetup.Visible = false;
                emailAccountControl.Visible = true;
                emailAccountControl.Top = panelNotSetup.Top;

                emailAccountControl.InitializeForStore(store);
            }
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            EmailAccountEntity account = GetStore<OrderMotionStoreEntity>().OrderMotionEmailAccount;
            if (account == null)
            {
                MessageHelper.ShowInformation(this, "You must setup an email account before continuing.");
                e.NextPage = this;
            }

            GetStore<OrderMotionStoreEntity>().StoreName = "OrderMotion Store";
        }

        /// <summary>
        /// Add an email account to shipworks
        /// </summary>
        private void OnAddAccount(object sender, EventArgs e)
        {
            OrderMotionStoreEntity store = GetStore<OrderMotionStoreEntity>();

            if (OrderMotionUtility.SetupEmailAccount(store, this))
            {
                addAccount.Enabled = false;
                pictureBoxSetup.Visible = true;
                labelSetup.Visible = true;
            }
        }
    }
}
