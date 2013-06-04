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

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WizardPages
{
    /// <summary>
    /// Setup wizard page for specifying which type of orders to download from ChannelAdvisor
    /// </summary>
    public partial class ChannelAdvisorDownloadCriteriaPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorDownloadCriteriaPage()
        {
            InitializeComponent();
        }

        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ChannelAdvisorStoreEntity store = GetStore<ChannelAdvisorStoreEntity>();

            if (!storeSettings.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }
        }
    }
}
