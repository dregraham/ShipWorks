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

        }
    }
}
