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
using Interapptive.Shared.Net;
using ShipWorks.UI;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.WizardPages
{
    /// <summary>
    /// Wizard page for setting up the Channel Advisor account key
    /// </summary>
    public partial class ChannelAdvisorAccountPage : AddStoreWizardPage
    {
        public ChannelAdvisorAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping next in the wizard
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ChannelAdvisorStoreEntity store = GetStore<ChannelAdvisorStoreEntity>();

            Cursor.Current = Cursors.WaitCursor;

            if (!accountSettings.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            store.StoreName = "ChannelAdvisor";
        }
    }
}
