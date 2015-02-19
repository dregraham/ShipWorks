using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using log4net;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Groupon.WizardPages
{
    /// <summary>
    /// Wizard page for configuring Volusion access
    /// </summary>
    public partial class GrouponStoreAccountPage : AddStoreWizardPage
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponStoreAccountPage));

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponStoreAccountPage()
        {
            InitializeComponent();
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
                GrouponWebClient client = new GrouponWebClient(store);
                client.GetOrders(1);
            }
            catch (GrouponException ex)
            {
                ShowConnectionException(ex);
                e.NextPage = this;
                return;
            }
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
