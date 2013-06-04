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
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Setup wizard page for specifying which type of orders to download from Amazon
    /// </summary>
    public partial class AmazonMwsDownloadCriteriaPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsDownloadCriteriaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the options page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            AmazonStoreEntity store = GetStore<AmazonStoreEntity>();

            excludeFba.Checked = store.ExcludeFBA;
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            AmazonStoreEntity store = GetStore<AmazonStoreEntity>();

            store.ExcludeFBA = excludeFba.Checked;
        }
    }
}
