using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Wizard page for ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.ChannelAdvisor)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class ChannelAdvisorAddStoreWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ChannelAdvisorAddStoreWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save excludeFba when leaving page
        /// </summary>
        private void OnStepNext(object sender, UI.Wizard.WizardStepEventArgs e)
        {
            excludeFba.SaveToEntity(GetStore<ChannelAdvisorStoreEntity>());
        }
    }
}
