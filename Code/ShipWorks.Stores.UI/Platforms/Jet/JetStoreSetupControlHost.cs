using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Jet
{
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Jet, ExternallyOwned = true)]
    [Order(typeof(WizardPage), Order.Unordered)]
    public partial class JetStoreSetupControlHost : AddStoreWizardPage
    {
        private readonly IJetStoreSetupControlViewModel viewModel;

        public JetStoreSetupControlHost(IJetStoreSetupControlViewModel viewModel)
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
            viewModel.Load(GetStore<JetStoreEntity>());
        }

        /// <summary>
        /// Called when [step next].
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!viewModel.Save(GetStore<JetStoreEntity>()))
            {
                e.NextPage = this;
            }
        }
    }
}
