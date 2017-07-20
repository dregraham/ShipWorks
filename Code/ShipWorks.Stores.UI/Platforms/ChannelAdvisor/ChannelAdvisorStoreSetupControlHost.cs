using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.ChannelAdvisor, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class ChannelAdvisorStoreSetupControlHost : AddStoreWizardPage
    {
        private readonly IChannelAdvisorAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorStoreSetupControlHost(IChannelAdvisorAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitializeComponent();

            storeSetupControl.DataContext = viewModel;
        }

        /// <summary>
        /// Called when stepping into this wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            viewModel.Load(GetStore<ChannelAdvisorStoreEntity>());
        }

        /// <summary>
        /// Called when [step next].
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<ChannelAdvisorStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}
