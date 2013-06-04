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

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Wizard page for configuring Amazon MWS
    /// </summary>
    public partial class AmazonMwsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Step Next
        /// </summary>
        private void OnStepNext(object sender, UI.Wizard.WizardStepEventArgs e)
        {
            AmazonStoreEntity amazonStore = GetStore<AmazonStoreEntity>();

            if (!storeSettingsControl.SaveToEntity(amazonStore))
            {
                e.NextPage = this;
            }

            // this store is using the new API now
            amazonStore.AmazonApi = (int) AmazonApi.MarketplaceWebService;
        }
    }
}
