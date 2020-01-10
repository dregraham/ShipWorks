using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Setup wizard page for specifying which type of orders to download from ChannelAdvisor
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class ChannelAdvisorExcludeFbaWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorExcludeFbaWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the options page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            ChannelAdvisorStoreEntity store = GetStore<ChannelAdvisorStoreEntity>();

            excludeFba.Checked = store.ExcludeFBA;
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ChannelAdvisorStoreEntity store = GetStore<ChannelAdvisorStoreEntity>();

            store.ExcludeFBA = excludeFba.Checked;
        }
    }
}
