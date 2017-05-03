using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.ShopSite.WizardPages
{
    /// <summary>
    /// Wizard page for setting up the login information for ShopSite
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.ShopSite)]
    [Order(typeof(WizardPage), 1)]
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
            store.Website = new Uri(store.ApiUrl).Host;
        }
    }
}
