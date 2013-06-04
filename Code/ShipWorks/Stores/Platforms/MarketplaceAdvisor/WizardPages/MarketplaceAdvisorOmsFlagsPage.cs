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
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    /// <summary>
    /// Wizard page for deciding which flags need to be set when a MarketplaceAdvisor order is downloaded
    /// </summary>
    public partial class MarketplaceAdvisorOmsFlagsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOmsFlagsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            if (store.AccountType != (int) MarketplaceAdvisorAccountType.OMS)
            {
                e.Skip = true;
                return;
            }
        }

        /// <summary>
        /// The page is now visible
        /// </summary>
        private void OnPageShown(object sender, EventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            flagsControl.LoadFlags(store);
        }

        /// <summary>
        /// Stepping next from the flags page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            store.DownloadFlags = (int) flagsControl.ReadFlags();
        }
    }
}
