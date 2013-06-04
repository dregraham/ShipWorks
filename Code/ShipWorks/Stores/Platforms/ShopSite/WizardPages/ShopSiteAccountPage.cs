using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ShopSite.WizardPages
{
    /// <summary>
    /// Wizard page for setting up the login information for ShopSite
    /// </summary>
    public partial class ShopSiteAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ShopSiteStoreEntity store = GetStore<ShopSiteStoreEntity>();

            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            store.StoreName = "ShopSite Store";
            store.Website = new Uri(store.CgiUrl).Host;
        }
    }
}
