using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Wizard page for ChannelAdvisor
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class ChannelAdvisorExcludeFbaWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ChannelAdvisorExcludeFbaWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save excludeFba when leaving page
        /// </summary>
        private void OnStepNext(object sender, ShipWorks.UI.Wizard.WizardStepEventArgs e)
        {
            excludeFba.SaveToEntity(GetStore<ChannelAdvisorStoreEntity>());
        }
    }
}
