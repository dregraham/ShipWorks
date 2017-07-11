using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.ChannelAdvisor, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class ChannelAdvisorStoreSetupControlHost : AddStoreWizardPage
    {
        private readonly IChannelAdvisorAccountSettingsViewModel viewModel;

        public ChannelAdvisorStoreSetupControlHost(IChannelAdvisorAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitializeComponent();

            storeSetupControl.DataContext = viewModel;
        }

        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<ChannelAdvisorStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}
